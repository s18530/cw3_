ALTER PROCEDURE PromoteStudents @Name varchar(100), @Semester int
AS 
BEGIN
DECLARE @Id int = (Select Max(IdEnrollment) + 1 FROM Enrollment)
DECLARE @IdStudy int = (Select Distinct Studies.IdStudy FROM Enrollment 
INNER JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy 
WHERE Name = @name)
				If NOT EXISTS (Select * FROM Enrollment
				INNER JOIN Studies ON Enrollment.IdStudy = Studies.idStudy
				WHERE Name = @Name AND Semester = @Semester +1)
			BEGIN
				INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (@Id, @Semester + 1, @IdStudy, GetDate())
				UPDATE Student SET Student.IdEnrollment = @Id WHERE Student.IdEnrollment = (SELECT DISTINCT FROM Enrollment
				INNER JOIN Studies ON Enrollment.IdStudy = Studies.idStudy
				WHERE Name = @name AND Semester = @Semester)
			END
		ELSE	
			BEGIN
				UPDATE Student SET IdEnrollment = @Id WHERE Student.IdEnrollment = (SELECT DISTINCT IdEnrollment FROM Enrollment
				INNER JOIN Studies ON Enrollment.IdStudy = Studies.idStudy
				WHERE Name = @Name AND Semester = @Semester)
			END
END; 

EXEC PromoteStudents 'Sztuka Nowych Mediów', 1



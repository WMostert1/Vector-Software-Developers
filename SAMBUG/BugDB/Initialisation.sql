/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SET DATEFORMAT YMD;

INSERT INTO [dbo].[Role] (RoleType, RoleDescription)
VALUES	(1, 'Grower'),
		(2, 'Admin');

INSERT INTO [dbo].[User] (Email, Password)
VALUES	('werner.mostert1@gmail.com', 'pass'),
		('test@gmail.com', 'pass1'),
		('abrie@gmail.com', 'pass2');

INSERT INTO [dbo].[Farm] (UserID, FarmName)
VALUES	(1, 'Werner Farm'),
		(3, 'Abrie Farm');

INSERT INTO [dbo].[Block] (FarmID, BlockName)
VALUES	(1, 'Block A'),
		(1, 'Block B'),
		(1, 'Block C'),
		(1, 'Block D'),
		(1, 'Block E'),
		(1, 'Block F'),
		(2, 'Test Z'),
		(2, 'Test Y'),
		(2, 'Test W'),
		(2, 'Test X'),
		(2, 'Test V'),
		(2, 'Test V');

INSERT INTO [dbo].[UserRole] (UserID, RoleID)
VALUES	(1, 1),
		(2, 2),
		(3, 1);

INSERT INTO [dbo].[Scoutstop] (UserID, BlockID, NumberOfTrees, Latitude, Longitude, Date)
VALUES	(1, 1, 8, -25.66, -25.66, '2005-08-02'),
		(1, 1, 3, -25.66, -25.66, '2007-02-24'),
		(1, 2, 3, -25.66, -25.66, '2010-12-22'),
		(1, 2, 1, -25.66, -25.66, '2014-11-30'),
		(1, 3, 5, -25.66, -25.66, '2011-03-19'),
		(1, 4, 2, -25.66, -25.66, '2009-04-01'),
		(1, 5, 3, -25.66, -25.66, '2002-05-05'),
		(1, 6, 9, -25.66, -25.66, '2015-07-07'),

		(3, 7, 8, -25.66, -25.66, '2005-08-26'),
		(3, 7, 3, -25.66, -25.66, '2007-02-24'),
		(3, 8, 3, -25.66, -25.66, '2010-12-22'),
		(3, 9, 1, -25.66, -25.66, '2014-11-30'),
		(3, 10, 5, -25.66, -25.66, '2011-03-19'),
		(3, 10, 2, -25.66, -25.66, '2009-04-01'),
		(3, 11, 3, -25.66, -25.66, '2002-05-05'),
		(3, 12, 9, -25.66, -25.66, '2015-07-07');

INSERT INTO [dbo].[Species] (SpeciesName, Lifestage, IdealPicture, isPest)
VALUES	('Coconut Bug', 1, CAST ('testPic' AS varbinary(MAX)), 1),
		('Coconut Bug', 2, CAST ('testPic1' AS varbinary(MAX)), 1),
		('Coconut Bug', 3, CAST ('testPic2' AS varbinary(MAX)), 1),
		('Coconut Bug', 4, CAST ('testPic3' AS varbinary(MAX)), 1),
		('Two Spotted Bug', 1, CAST ('testPic' AS varbinary(MAX)), 1),
		('Two Spotted Bug', 2, CAST ('testPic1' AS varbinary(MAX)), 1),
		('Two Spotted Bug', 3, CAST ('testPic2' AS varbinary(MAX)), 1),
		('Two Spotted Bug', 4, CAST ('testPic3' AS varbinary(MAX)), 1),
		('Yellow Edged Bug', 1, CAST ('testPic' AS varbinary(MAX)), 1),
		('Yellow Edged Bug', 2, CAST ('testPic1' AS varbinary(MAX)), 1),
		('Yellow Edged Bug', 3, CAST ('testPic2' AS varbinary(MAX)), 1),
		('Yellow Edged Bug', 4, CAST ('testPic3' AS varbinary(MAX)), 1);

INSERT INTO [dbo].[ScoutBug] (ScoutStopID, SpeciesID, NumberOfBugs, FieldPicture, Comments)
VALUES	(1, 10, 6, CAST ('testPic' AS varbinary(MAX)), 'I was not sure how many bugs there were, 6 was an estimation.'),
		(2, 8, 3, CAST ('testPic1' AS varbinary(MAX)), 'I keep on finding these bugs, we should spray'),
		(3, 2, 12, CAST ('testPic2' AS varbinary(MAX)), 'Check out how they work together.'),
		(4, 10, 6, CAST ('testPic' AS varbinary(MAX)), 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce mattis eget augue quis eleifend. Donec vel ipsum imperdiet, sollicitudin nisl ac, dapibus purus'),
		(5, 7, 100, CAST ('testPic1' AS varbinary(MAX)), 'Praesent tempor, felis efficitur efficitur facilisis, tortor mauris hendrerit eros, nec feugiat erat urna nec turpis'),
		(6, 4, 75, CAST ('testPic' AS varbinary(MAX)), 'Fusce turpis eros, pharetra in mollis ut, luctus sed nisi. Donec sit amet semper odio, in lobortis nunc.'),
		(7, 6, 90, CAST ('testPic1' AS varbinary(MAX)), 'Donec sit amet semper odio, in lobortis nunc. Mauris lacinia dui et velit aliquet auctor ac pulvinar est.'),
		(8, 1, 12, CAST ('testPic2' AS varbinary(MAX)), 'Suspendisse hendrerit eros eget tincidunt mattis.'),

		(9, 10, 6, CAST ('testPic' AS varbinary(MAX)), 'I was not sure how many bugs there were, 6 was an estimation.'),
		(10, 8, 3, CAST ('testPic1' AS varbinary(MAX)), 'I keep on finding these bugs, we should spray'),
		(11, 2, 12, CAST ('testPic2' AS varbinary(MAX)), 'Check out how they work together.'),
		(12, 10, 6, CAST ('testPic' AS varbinary(MAX)), 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce mattis eget augue quis eleifend. Donec vel ipsum imperdiet, sollicitudin nisl ac, dapibus purus'),
		(13, 7, 100, CAST ('testPic1' AS varbinary(MAX)), 'Praesent tempor, felis efficitur efficitur facilisis, tortor mauris hendrerit eros, nec feugiat erat urna nec turpis'),
		(14, 4, 75, CAST ('testPic' AS varbinary(MAX)), 'Fusce turpis eros, pharetra in mollis ut, luctus sed nisi. Donec sit amet semper odio, in lobortis nunc.'),
		(15, 6, 90, CAST ('testPic1' AS varbinary(MAX)), 'Donec sit amet semper odio, in lobortis nunc. Mauris lacinia dui et velit aliquet auctor ac pulvinar est.'),
		(16, 1, 12, CAST ('testPic2' AS varbinary(MAX)), 'Suspendisse hendrerit eros eget tincidunt mattis.');

INSERT INTO [dbo].[Treatment] (BlockID, Date, Comments)
VALUES	(10, '2008-05-23', 'Sprayed with xxxx'),
		(2, '2010-11-02', 'Sprayed with yyyy'),
		(2, '2005-07-04', 'Sprayed with zzzz'),
		(1, '2013-01-01', 'Sprayed with hhhh'),
		(4, '2004-02-02', 'Sprayed with kkkk'),
		(5, '2009-03-03', 'Sprayed with aaaa'),
		(7, '2007-04-04', 'Sprayed with rrrr'),
		(8, '2001-05-05', 'Sprayed with oooo'),
		(3, '2009-06-06', 'Sprayed with cccc'),
		(4, '2011-07-07', 'Sprayed with mmmm'),
		(6, '2012-08-08', 'Sprayed with vvvv'),
		(6, '2015-09-09', 'Sprayed with qqqq'),
		(9, '2007-10-10', 'Sprayed with dddd'),
		(11, '2005-11-11', 'Sprayed with llll'),
		(12, '2006-12-15', 'Sprayed with jjjj');
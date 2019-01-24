/* CREATE TABLE IF NOT EXISTS Classification (
	id int PRIMArY KEY AUTO_INCREMENT,
    title varchar(30) NOT NULL,
		UNIQUE (title)
); */

INSERT INTO Classification(title) VALUES ('High School - Freshman');
INSERT INTO Classification(title) VALUES ('High School - Sophomore');
INSERT INTO Classification(title) VALUES ('High School - Junior');
INSERT INTO Classification(title) VALUES ('High School - Senior');
INSERT INTO Classification(title) VALUES ('High School - Instructor');
INSERT INTO Classification(title) VALUES ('College - Freshman');
INSERT INTO Classification(title) VALUES ('College - Sophomore');
INSERT INTO Classification(title) VALUES ('College - Junior');
INSERT INTO Classification(title) VALUES ('College - Senior');
INSERT INTO Classification(title) VALUES ('College - Masters Student');
INSERT INTO Classification(title) VALUES ('College - Ph.D. Student');
INSERT INTO Classification(title) VALUES ('College - Instructor');
INSERT INTO Classification(title) VALUES ('Other');

/* CREATE TABLE IF NOT EXISTS Events (
	id int PRIMARY KEY AUTO_INCREMENT,
    title varchar(30) NOT NULL,
		UNIQUE (title)
); */

INSERT INTO Events (title) VALUES ('Login');
INSERT INTO Events (title) VALUES ('Change Scene');
INSERT INTO Events (title) VALUES ('Checkpoint');
INSERT INTO Events (title) VALUES ('Test Log');

/* CREATE TABLE IF NOT EXISTS Users (
	id int PRIMARY KEY AUTO_INCREMENT,
    username varchar(70) NOT NULL,
		UNIQUE(email),
    classification int,
		FOREIGN KEY (classification) REFERENCES Classification(id)
); */

INSERT INTO Users (username, classification) VALUES ('rlafferty2', (SELECT id FROM Classification WHERE title LIKE '%Masters%'));
INSERT INTO Users (username, classification) VALUES ('rclafferty', (SELECT id FROM Classification WHERE title LIKE '%Other%'));
INSERT INTO Users (username, classification) VALUES ('test', (SELECT id FROM Classification WHERE title LIKE '%Other%'));

/* CREATE TABLE IF NOT EXISTS UserLogs (
	userID int, # Foreign Keys are not null by definition
		FOREIGN KEY (userID) REFERENCES Users(id),
	levelID int NOT NULL,
    eventID int, # Foreign Keys are not null by definition
		FOREIGN KEY (eventID) REFERENCES Events(id),
	currentScore int NOT NULL,
	comment varchar(300) NOT NULL,
    time datetime NOT NULL
); */

INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test', '2019-01-17 00:00:01');
INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test2', '2019-01-17 00:00:02');
INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test3', '2019-01-17 00:00:03');
INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test4', '2019-01-17 00:00:04');
INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test4', '2019-01-17 00:00:05');
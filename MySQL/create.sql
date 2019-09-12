USE testthesisdatabase;

DROP TABLE IF EXISTS UserLogs;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Events;
DROP TABLE IF EXISTS UserSchoolClassification;

DROP TABLE IF EXISTS Letters;

CREATE TABLE IF NOT EXISTS UserSchoolClassification (
	id int PRIMARY KEY AUTO_INCREMENT,
    title varchar(30) NOT NULL,
		UNIQUE KEY (title)
);

CREATE TABLE IF NOT EXISTS Events (
	id int PRIMARY KEY AUTO_INCREMENT,
    title varchar(30) NOT NULL,
		UNIQUE KEY (title)
);

CREATE TABLE IF NOT EXISTS Users (
	id int PRIMARY KEY AUTO_INCREMENT,
    username varchar(70) NOT NULL,
		UNIQUE KEY (username),
    classification int, # Foreign Keys are not null by definition
		FOREIGN KEY (classification) REFERENCES UserSchoolClassification(id)
);

CREATE TABLE IF NOT EXISTS UserLogs (
	userID int, # Foreign Keys are not null by definition
		FOREIGN KEY (userID) REFERENCES Users(id),
	levelID int NOT NULL,
    eventID int, # Foreign Keys are not null by definition
		FOREIGN KEY (eventID) REFERENCES Events(id),
	currentScore int NOT NULL,
	comment varchar(300) NOT NULL,
    time datetime NOT NULL,

	UNIQUE KEY (userID, time)
);

CREATE TABLE IF NOT EXISTS Letters (
	id int PRIMARY KEY AUTO_INCREMENT,
    sender varchar(50) NOT NULL,
    receiver varchar(50) NOT NULL,
    message varchar(500) NOT NULL,
    
    UNIQUE KEY (id, sender, receiver)
);

# Populate
INSERT INTO UserSchoolClassification(title) VALUES ('High School - Freshman');
INSERT INTO UserSchoolClassification(title) VALUES ('High School - Sophomore');
INSERT INTO UserSchoolClassification(title) VALUES ('High School - Junior');
INSERT INTO UserSchoolClassification(title) VALUES ('High School - Senior');
INSERT INTO UserSchoolClassification(title) VALUES ('High School - Instructor');
INSERT INTO UserSchoolClassification(title) VALUES ('College - Freshman');
INSERT INTO UserSchoolClassification(title) VALUES ('College - Sophomore');
INSERT INTO UserSchoolClassification(title) VALUES ('College - Junior');
INSERT INTO UserSchoolClassification(title) VALUES ('College - Senior');
INSERT INTO UserSchoolClassification(title) VALUES ('College - Masters Student');
INSERT INTO UserSchoolClassification(title) VALUES ('College - Ph.D. Student');
INSERT INTO UserSchoolClassification(title) VALUES ('College - Instructor');
INSERT INTO UserSchoolClassification(title) VALUES ('Other');

INSERT INTO Events (title) VALUES ('Login');
INSERT INTO Events (title) VALUES ('Change Scene');
INSERT INTO Events (title) VALUES ('Checkpoint');
INSERT INTO Events (title) VALUES ('Test Log');

INSERT INTO Users (username, classification) VALUES ('rlafferty2', (SELECT id FROM UserSchoolClassification WHERE title LIKE '%Masters%'));
INSERT INTO Users (username, classification) VALUES ('rclafferty', (SELECT id FROM UserSchoolClassification WHERE title LIKE '%Other%'));
INSERT INTO Users (username, classification) VALUES ('test', (SELECT id FROM UserSchoolClassification WHERE title LIKE '%Other%'));

INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test', '2019-01-17 00:00:01');
INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test2', '2019-01-17 00:00:02');
INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test3', '2019-01-17 00:00:03');
INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test4', '2019-01-17 00:00:04');
INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ((SELECT id FROM Users WHERE username LIKE '%test%'), -1, (SELECT id FROM Events WHERE title LIKE '%test log%'), -999, 'This is a test4', '2019-01-17 00:00:05');
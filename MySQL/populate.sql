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
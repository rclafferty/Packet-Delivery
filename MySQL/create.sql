CREATE TABLE IF NOT EXISTS Classification (
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
		FOREIGN KEY (classification) REFERENCES Classification(id)
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
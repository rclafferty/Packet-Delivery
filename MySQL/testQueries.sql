SELECT u.username as 'Name', l.levelID as 'Level ID', e.title as 'Event Title', l.currentScore as 'Score', l.comment as 'Log Message', l.time as 'Log Time' FROM UserLogs l
	JOIN Users u ON l.userID = u.id
    JOIN Events e ON l.eventID = e.id;
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <link rel="stylesheet" href="css/styles.css">
    <title>Instructor View</title>
</head>
<body>
    <h1>[Game Title] Logs:</h1>

    <?php
        include 'dbconnection.php';

        // See if user exists
        $query = "SELECT u.username as 'Name', l.levelID as 'Level ID', e.title as 'Event Title', l.currentScore as 'Score', l.comment as 'Log Message', l.time as 'Log Time' FROM UserLogs l JOIN Users u ON l.userID = u.id JOIN Events e ON l.eventID = e.id";
        $result = mysqli_query($dblink, $query);
        $num_rows = $result->num_rows;
        $num_cols = 6;

        echo "<h3>Number of logs: $num_rows</h3>";

        $tf = false;
        $error = "none";

        // If successfully retrieved user info from database
        if ($num_rows > 0)
        {
            $tf = true;

            echo "<table style='width:100%'>";
            echo "<tr>";
            echo "<th>Username</th>";
            echo "<th>Level ID</th>";
            echo "<th>Event Title</th>";
            echo "<th>Score</th>";
            echo "<th>Log Message</th>";
            echo "<th>Log Time</th>";
            echo "</tr>";

            while ($row = mysqli_fetch_row($result))
            {
                echo "<tr>";
                for ($i = 0; $i < $num_cols; $i++)
                {
                    $value = $row[$i];
                    echo "<td>$value</td>";
                }
                echo "</tr>";
            }
        }
        else
        {
            $tf = false;
            $error = "Invalid query";
            echo "<p>No Logs</p>";
        }
    ?>
</body>
</html>
<?php
    include_once 'dbconnection.php';
    
    // Get the values from the post parameters
    $user = $_GET['user'];
    $level = $_GET['level'];
    $event = $_GET['event'];
    $score = $_GET['score'];
    $comment = $_GET['comment'];

    // Clean email/pass of SQL Injection tags
    $user = strip_tags($user);
    $level = strip_tags($level);
    $event = strip_tags($event);
    $score = strip_tags($score);
    $comment = strip_tags($comment);
    // $time = strip_tags($time);

    // See if user exists
    $query = "SELECT * FROM Users WHERE username='$user'";
    $result = mysqli_query($dblink, $query);
    $row = mysqli_fetch_row($result);

    $tf = false;
    $error1 = "none";
    $error2 = "none";

    // If successfully retrieved user info from database
    if ($row)
    {
        $tf = true;
        
        $userID = $row[0];
        // echo $value . " ";

        // Insert log into database
        $query2 = "INSERT INTO UserLogs (userID, levelID, eventID, currentScore, comment, time) VALUES ($userID, $level, $event, $score, '$comment', NOW())";
        // $query2 = "INSERT INTO Users(username, classification) VALUES ('test insert from log.php', (SELECT id FROM Classification WHERE title LIKE '%other%'))";
        $result2 = mysqli_query($dblink, $query2);
        $row2 = mysqli_fetch_row($result2);

        if ($row2)
        {
            // Error --> Inserts don't return values
            $error2 = "Insert failed";
            $error2 = $row2;
        }
        else
        {
            // Success
        }
        $error2 = $row2;
    }
    else
    {
        $tf = false;
        $error1 = "Invalid username";
        $error2 = "Did not execute insert";
    }
        
    $dataArray = array('success' => $tf, 'error_seek' => $error1, 'error_store' => $error2, 'username' => $user, 'userID' => $userID, 'level' => $level, 'event' => $event, 'score' => $score, 'message' => $comment, 'time' => $time);

    header('Content-Type: application/json');
    echo json_encode($dataArray);
    // echo "<p>User = $user</p>";
    // echo "<p>level = $level</p>";
    // echo "<p>event = $event</p>";
    // echo "<p>score = $score</p>";
    // echo "<p>comment = $comment</p>";
    // echo "<p>time = $time</p>";
?>
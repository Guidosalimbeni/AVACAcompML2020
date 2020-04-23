<?php
    // Configuration

    //echo "hello";

    $servername = "mysql.guidosalimbeni.it";
    $server_username = "guidosalimbeni";
    $server_password = "1.oliver";
    $database = "compml"; // Might look something like (ActName_DatabaseName)
    $table    = "players";


    // These variables are sent from Unity, we access them via
    // $_POST and make sure to santitize the input to mysql.
    
    $name                  = $_POST['name'];
    $filenameImg           = $_POST['filenameImg'];
    $scoreBoundsBalance    = $_POST['scoreBoundsBalance'];
    $scorePixelsBalance    = $_POST['scorePixelsBalance'];
    $scoreUnityVisual      = $_POST['scoreUnityVisual'];

	$g0      = $_POST['g0'];
	$g1      = $_POST['g1'];
	$g2      = $_POST['g2'];
	$g3      = $_POST['g3'];
	$g4      = $_POST['g4'];
	$g5      = $_POST['g5'];

	$ImagePixelsList = $_POST['ImagePixelsList'];

	$judge      = $_POST['judge'];



	/*
	$name                  = 'bla';
    $filenameImg           = 'blo';
    $scoreBoundsBalance    = 5;
    $scorePixelsBalance    = 6;
    $scoreUnityVisual      = 7;
	*/


	$connection = mysql_connect($servername, $server_username, $server_password);

	if(! $connection ) {
      die('Could not connect: ' . mysql_error());
	}

    // These are the MySQL queries that we are going to use when
    // we store our new score, and return our top 10 players.
    
    $insert   = "INSERT INTO $table (`id`, `name`, `filenameImg`, `scoreBoundsBalance`, `scorePixelsBalance`, `scoreUnityVisual` , `g0` , `g1` , `g2` , `g3` , `g4` , `g5`, `ImagePixelsList` , `judge`) 
	VALUES (NULL, '$name', '$filenameImg', '$scoreBoundsBalance', '$scorePixelsBalance' , '$scoreUnityVisual' , '$g0' , '$g1' , '$g2' , '$g3' , '$g4' , '$g5', '$ImagePixelsList' , '$judge')";
                     
    //$select   = "SELECT * FROM $table WHERE game='$game' ORDER BY score DESC LIMIT 10";
	
    // o--------------------------------------------------------
    // | Access database
    // o--------------------------------------------------------

    // Connect to the server with our settings defined above.

    

    // 1. Select the database to work with. 
    // 2. Insert our new player 
    
    mysql_select_db($database); 
    $result = mysql_query($insert, $connection);

	if(! $result ) {
      die('Could not enter data: ' . mysql_error());
   }

    //$result = mysql_query($select, $connection);

// Finally, go through top 10 players and return the result
    // back to Unity. The output format for each line will be: 
    // {game}:{player}:{score}
    
    //while ($row = mysql_fetch_array($result))
        //echo $row['game']   . ":" . $row['player'] . ":" . $row['score']  . "\n";
  
    // Close the connection, we're done here.
    
    //echo "received your data thank you";

    mysql_close($connection);




?>
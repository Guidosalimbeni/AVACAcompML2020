<?php
    //You need to fill in this information!
    $db = mysql_connect("mysql.guidosalimbeni.it", "guidosalimbeni", "1.oliver") or die('Failed to connect: ' . mysql_error()); 
		
	mysql_select_db("compml") or die('Failed to access database');
 

	$name                  = mysql_real_escape_string($_GET['name']); 
	$filenameImg           = mysql_real_escape_string($_GET['filenameImg']); 
    $scoreBoundsBalance    = mysql_real_escape_string($_GET['scoreBoundsBalance']); 
    $scorePixelsBalance    = mysql_real_escape_string($_GET['scorePixelsBalance']); 
    $scoreUnityVisual      = mysql_real_escape_string($_GET['scoreUnityVisual']); 

	$g0      = mysql_real_escape_string($_GET['g0']); 
	$g1      = mysql_real_escape_string($_GET['g1']); 
	$g2      = mysql_real_escape_string($_GET['g2']); 
	$g3      = mysql_real_escape_string($_GET['g3']); 
	$g4      = mysql_real_escape_string($_GET['g4']); 
	$g5      = mysql_real_escape_string($_GET['g5']); 

	$judge      = mysql_real_escape_string($_GET['judge']); 


      $query =  "SELECT * FROM `players`";
	  

      $result = mysql_query($query) or die('Query failed: ' . mysql_error());
      
      //This is more elaborate than we need, considering we're only grabbing one rank, but you can modify it if needs be.
      $num_results = mysql_num_rows($result);  
      
      for($i = 0; $i < $num_results; $i++)
      {
           $row = mysql_fetch_array($result);
           echo $row['name'] . "\t" . $row['g0'] . "\t" . $row['g1']. "\t" . $row['g2']. "\t" . $row['g3']
		   . "\t" . $row['g4']. "\t" . $row['g5']. "\t" . $row['judge']. "\n";
      }

?>
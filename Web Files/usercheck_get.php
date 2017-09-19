<?php
$link = mysqli_connect('localhost','root','');
$database = mysqli_select_db($link,'mybb');

// Create connection
/*$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} */

$user = $_GET['username'];

$sql = "SELECT * FROM forums_users WHERE username = '". mysqli_real_escape_string($link,$user) ."'" ;
//$sql = "SELECT * FROM forums_users";
$result = $link->query($sql);

if ($result->num_rows > 0) {
	// Outputting the rows
	while($row = $result->fetch_assoc()) {
		
		echo $row['usergroup'];
		echo $row['additionalgroups'];
				
	}
} 
else
{
	echo "No user with the username " . $user;
}
//2: Registered <br> 3: Super Moderator <br> 4: Administrators <br> 5: Awaiting Activation <br> 6: Moderator <br> 7: Banned <br> 8: Premium
?>

<html>
<head>
<title>PHP account check return</title>
<meta http-equiv="refresh" content="1" />
</head>

<body>
<!-- <br><br><a href="/usercheck.php">Return</a> -->
</body>
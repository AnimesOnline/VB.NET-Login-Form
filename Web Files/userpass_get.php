<?php
$link = mysqli_connect('localhost','root','');
$database = mysqli_select_db($link,'mybb');

$user = $_GET['username'];
$password = $_GET['password'];

$sql = "SELECT * FROM forums_users WHERE username = '". mysqli_real_escape_string($link,$user) ."'" ;
$result = $link->query($sql);

if ($result->num_rows > 0) {
	// Outputting the rows
	while($row = $result->fetch_assoc()) {
		
		$password = $row['password'];
		$salt = $row['salt'];
		$plain_pass = $_GET['password'];
		$stored_pass = md5(md5($salt).md5($plain_pass));
		
		if($stored_pass == $row['password'])
		{
			echo "1";
		}
		else
		{
			echo "0";
		}
	}
} 
else
{
	echo "No user with the username " . $user;
}
/*

*/
?>

<html>
<head>
<title>PHP account check return</title>
<meta http-equiv="refresh" content="1" />
</head>

<body>
</body>

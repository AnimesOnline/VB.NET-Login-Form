<html>

<head>
<title>PHP account check</title>

<style type="text/css">
a {
    color: #0060B6;
    text-decoration: none;
}

a:hover 
{
     color:#00A0C6; 
     text-decoration:none; 
     cursor:pointer;  
}

#footer
{
    clear: both;
	background: white;
    padding: 0;
	border: 1px groove #aaaaaa;
    text-align: center;
    vertical-align: 10px;
    line-height: normal;
    margin: 0;
    position: fixed;
    bottom: 0px;
	left: -1px;
    width: 100%;
	height: 60px;
}

#text
{
    clear: both;
    padding: 0;
    text-align: center;
    vertical-align: middle;
    line-height: normal;
    margin: 0;
    position: relative;
    top: 70px;
	left: -10px;
    width: 100%;
	padding-bottom: 40px;
}

input{
   text-align:center;
}
</style>

</head>

<body>

<div class="text" id="text">
<font color="black" size="5"><b>Tyson's MyBB "API" v1.0.0.0</b></font>
	<br>
	<br>
	<form action="usercheck_get.php" method="get">
		<input type="text" name="username" id="username" placeholder="Username to check"><br>
		<div style="padding-top: 7px;"></div>
		<input name="submit" id="submit" type="submit">
	</form>
	<p>Enter the username you want to check.<br>It will return if they user is banned or not, their username, usergroup and additional groups.<br>It then checks the groups to see if the user is premium and returns the value.</p>
	<p>How to config: Go to usergroup_get.php and open it.<br>Change the $link parameters to what you need and do the same for $database.<br>The usergroup for banned SHOULD be the same, but the premium one might be different.<br>Just change 8 to whatever number you need.</p>
	<p>Source download: <a target="_blank" href="/mybbapi.zip">Click here</a></p>
</div>

<div class="footer" id="footer"><p>Copyright &copy Tyson Prefontaine-McRae 2017</p></div>

</body>

</html>
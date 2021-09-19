<?php
//
// $argv is not undefined! It contains the parameters passed to this script!
// @See - https://www.php.net/manual/en/reserved.variables.argv.php
//
$tische = $argv;

$htmlOutput = "<div class='mainWrapper'>";

foreach ($tische as $key => $schuelerzuordnung) {
	$htmlOutput .= "
		<div class='table'>
			<p class='table_headline'
			>" . "Tisch-" . ($key + 1) . "</p>
	";
	foreach ($schuelerzuordnung as $schueler) {
		$htmlOutput .= "<div class='place'><span class='student_name'>$schueler</span></div>";
	}
	$htmlOutput .= "
		</div>
	";
}
$htmlOutput .= "</div>";

$html = "
	<head>
		<title>Sitzplan</title>
		<html lang='de'>
		<meta charset='UTF-8'>
		<link rel='stylesheet' href='styles.css'>
	</head>
	<body>
		$htmlOutput
	</body>
";

return $html;
?>
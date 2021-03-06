﻿# Replaces debug appsettings with release appsettings if they exist
# TODO: Stop being an idiot and use HostBuilder
$releaseAppSettings = ".\appsettings.release.json"
$debugAppSettings = ".\appsettings.json"
if (Test-Path $releaseAppSettings)
{
	Write-Output 'Converting release json...'
	Remove-Item $debugAppSettings
	Rename-Item -Path $releaseAppSettings -NewName 'appsettings.json'
	Write-Output 'Done!'
}
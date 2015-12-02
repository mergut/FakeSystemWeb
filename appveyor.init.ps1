#
# Initialize AppVeyor build environment
#

# Release from tag:
#  Parse tag version,
#   ASSEMBLY_VERSION				= 1.2.3.0
#   ASSEMBLY_FILE_VERSION			= 1.2.3.{build}
#   ASSEMBLY_INFORMATIONAL_VERSION	= 1.2.3 or 1.2.3-rc

# CI build:
#  Use version from appveyor.yml,
#   ASSEMBLY_VERSION				= 1.2.3.0
#   ASSEMBLY_FILE_VERSION			= 1.2.3.{build}
#   ASSEMBLY_INFORMATIONAL_VERSION	= 1.2.3-ci{build}

$build = [int]$env:APPVEYOR_BUILD_NUMBER

if ($env:APPVEYOR_REPO_TAG -eq "True")
{
	('Building tag: ' + $env:APPVEYOR_REPO_TAG_NAME)

	$version = new-object Version ($env:APPVEYOR_REPO_TAG_NAME -replace '[^0-9.]', '')

	$ASSEMBLY_VERSION = $version.ToString(3) + '.0'
	$ASSEMBLY_FILE_VERSION = $version.ToString(3) + '.' + $build
	$ASSEMBLY_INFORMATIONAL_VERSION = $env:APPVEYOR_REPO_TAG_NAME
}
else
{
	('Building commit: ' + $env:APPVEYOR_REPO_COMMIT)

	$version = new-object Version $env:APPVEYOR_BUILD_VERSION

	$ASSEMBLY_VERSION = $version.ToString(3) + '.0'
	$ASSEMBLY_FILE_VERSION = $version.ToString(4)
	$ASSEMBLY_INFORMATIONAL_VERSION = $version.ToString(3) + '-ci' + $build.ToString('0000')
}

('ASSEMBLY_VERSION: ' + $ASSEMBLY_VERSION)
('ASSEMBLY_FILE_VERSION: ' + $ASSEMBLY_FILE_VERSION)
('ASSEMBLY_INFORMATIONAL_VERSION: ' + $ASSEMBLY_INFORMATIONAL_VERSION)

Update-AppveyorBuild -Version $ASSEMBLY_INFORMATIONAL_VERSION
Set-AppveyorBuildVariable -Name 'ASSEMBLY_VERSION' -Value $ASSEMBLY_VERSION
Set-AppveyorBuildVariable -Name 'ASSEMBLY_FILE_VERSION' -Value $ASSEMBLY_FILE_VERSION
Set-AppveyorBuildVariable -Name 'ASSEMBLY_INFORMATIONAL_VERSION' -Value $ASSEMBLY_INFORMATIONAL_VERSION

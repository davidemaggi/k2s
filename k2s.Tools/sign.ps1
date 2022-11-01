$baseRoot="D:\a\k2s\k2s\k2s.Tools\"

Set-Location $baseRoot


choco install windows-sdk-10-version-2104-all -y
#choco install gh -y

#Get-ChildItem "C:\Program Files (x86)\Windows Kits\10\bin\"
$SignTool= "C:\Program Files (x86)\Windows Kits\10\bin\10.0.20348.0\x64\signtool.exe"


Invoke-WebRequest -URI "$env:CERTIFICATE" -OutFile "cert.pfx"

$tag=$env:RELEASE

$x64File="k2s-$($tag)-win-x64.zip"
$x64Url= "https://github.com/davidemaggi/k2s/releases/download/$($tag)/$($x64File)"

$x86File="k2s-$($tag)-win-x86.zip"
$x86Url= "https://github.com/davidemaggi/k2s/releases/download/$($tag)/$($x86File)"

# Downloaad the binaries
Invoke-WebRequest -URI $x64Url -OutFile $x64File
Invoke-WebRequest -URI $x86Url -OutFile $x86File

# Extract the binaries
$x86Dir=$x86File.replace('.zip','')
$x64Dir=$x64File.replace('.zip','')


Expand-Archive -LiteralPath $x64File -DestinationPath $x64Dir
Expand-Archive -LiteralPath $x86File -DestinationPath $x86Dir

# Sign the Exe File

& $SignTool sign /f "cert.pfx" /p "$env:CODE_SIGN"  $x64Dir"\k2s.exe"
& $SignTool sign /f "cert.pfx" /p "$env:CODE_SIGN" $x86Dir"\k2s.exe"

Compress-Archive -Path $x64Dir\* -DestinationPath $x64Dir"-signed.zip"
Compress-Archive -Path $x86Dir\* -DestinationPath $x86Dir"-signed.zip"



$md5x64= Get-FileHash $x64Dir"-signed.zip" -Algorithm MD5
$md5x86= Get-FileHash $x86Dir"-signed.zip" -Algorithm MD5

Out-File -FilePath $x64Dir"-signed.zip.md5" -InputObject $md5x64
Out-File -FilePath $x86Dir"-signed.zip.md5" -InputObject $md5x86

gh release upload $tag $x64Dir"-signed.zip" --clobber
gh release upload $tag $x64Dir"-signed.zip.md5" --clobber
gh release upload $tag $x86Dir"-signed.zip" --clobber
gh release upload $tag $x86Dir"-signed.zip.md5" --clobber


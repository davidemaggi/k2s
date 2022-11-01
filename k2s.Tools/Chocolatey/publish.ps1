$baseRoot="D:\a\k2s\k2s\k2s.Tools\Chocolatey\"

Set-Location $baseRoot


choco install windows-sdk-10-version-2104-all -y
#choco install gh -y

choco apikey --key $env:CHOCO_TOKEN --source https://push.chocolatey.org/
#Get-ChildItem "C:\Program Files (x86)\Windows Kits\10\bin\"


$tag=$env:RELEASE
$tagStrip=$tag.substring(1)

$x64File="k2s-$($tag)-win-x64-signed.zip"
$x64Url= "https://github.com/davidemaggi/k2s/releases/download/$($tag)/$($x64File)"

$x86File="k2s-$($tag)-win-x86-signed.zip"
$x86Url= "https://github.com/davidemaggi/k2s/releases/download/$($tag)/$($x86File)"

# Downloaad the binaries
Invoke-WebRequest -URI $x64Url -OutFile $x64File
Invoke-WebRequest -URI $x86Url -OutFile $x86File

#Create Choco Files
$nuspec=Get-Content -Path .\tmp.nuspec
$chocoScript=Get-Content -Path .\tools\chocoTmp.ps1

$nuspec=$nuspec.Replace("@@VERSION@@",$tagStrip)+""

Out-File -FilePath .\k2s.nuspec -InputObject $nuspec


$md5x64= Get-FileHash $x64File -Algorithm MD5
$md5x86= Get-FileHash $x86File -Algorithm MD5

$chocoScript=$chocoScript.Replace("@@VERSION@@",$tag)+""
$chocoScript=$chocoScript.Replace("@@HASH_X64@@",$md5x64.Hash)+""
$chocoScript=$chocoScript.Replace("@@HASH_X86@@",$md5x86.Hash)+""

Out-File -FilePath .\tools\chocolateyinstall.ps1 -InputObject $chocoScript

Remove-Item -Path .\tools\chocoTmp.ps1

choco pack k2s.nuspec
$nupkg="k2s."+$tagStrip+".nupkg"
choco push $nupkg --source https://push.chocolatey.org/


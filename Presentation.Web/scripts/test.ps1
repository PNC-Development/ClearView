Param ([string] $path);
$a = test-path -path $path;
New-Variable -Name result;
$result = New-Object PSObject;
Add-Member -InputObject $result -MemberType NoteProperty -Name ResultCode -Value $null;
Add-Member -InputObject $result -MemberType NoteProperty -Name Message -Value $null;
if ($a -eq $true )
{
$result.ResultCode = 0;
$result.Message = "Its there";
#exit 0;
}
else
{
$result.ResultCode = 1;
$result.Message = "Notty";
#exit 1
};
return ($result);

  
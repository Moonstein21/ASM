$SqlServer = "AOG-MSA";
$SqlCatalog = "MvcRequestContext-1";
$SqlLogin = "sa";
$SqlPassw = "Gfhjkm123"
$SqlConnection = New-Object System.Data.SqlClient.SqlConnection
$SqlConnection.ConnectionString = "Server=$SqlServer; Database=$SqlCatalog; User ID=$SqlLogin; Password=$SqlPassw;"
$SqlConnection.Open()
#формируем список пользователей с именами и email адресами
Import-Module activedirectory

$UserList = Get-ADUser -filter {Enabled -eq $true} -properties  DisplayName, mailNickname, DistinguishedName, MemberOf
ForEach($user in $UserList)
{
$name= $user.DisplayName;
$login = $user.mailNickname;
$address = $user.DistinguishedName

if ($login)
{
$GroupList=Get-ADPrincipalGroupMembership $user.mailNickname | select name
$group = "";
ForEach($groups in $GroupList)
{
$group= $group + $groups.name;
}

$SqlCmd = $SqlConnection.CreateCommand()
$SqlCmd.CommandText = "INSERT INTO DLPDark (name,login,gro,address) VALUES ('$name','$login','$group','$address')"
$objReader = $SqlCmd.ExecuteReader()
$objReader.close()

}

}
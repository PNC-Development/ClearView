Option Explicit

' Decalre Variables ---
Dim Description							' Username Safe
Dim NewSafe								' Newly Created Safe
Dim PrivateArk							' Private Ark Object DLL
Dim Safe								' Safe Object
Dim SafeName							' Safe Name
Dim User								' Safe User ID
Dim Vault								' Vault Server Object
Dim XID									' AD XID

' Declare Constants ---
Const WebAccess = 5
Const SharedAccount = "PVWAGWAccounts"
Const FullAccess = 1023 	' Admin,Backup,Confirm,Delete,ManageOwners,Monitor,
							' NoConfirmRequired,None,Retrieve,Store,ValidateSafeContent
Const DefaultAccess = 15 	' Store,Retrieve,Monitor,Store
Const AdminMon = 129		' Admin,Monitor
Const Administrator = 128 	' Admin
Const Backup = 16 			' Backup
Const Confirm = 32			' Confirm
Const Delete = 8			' Delete
Const ManageOwners = 64		' Manage Owners
Const Monitor = 1			' Monitor
Const NoConfirmation = 256	' No Confirmation Required
Const None = 0				' No Access
Const Retrieve = 2			' Retrieve
Const Store = 4				' Store
Const ValidateContent = 512	' Validate Safe Content

' Set Variables ---
XID = "XJCK03Y"

' Set Objects --- 
Set PrivateArk = CreateObject("XAPI.PrivateArk")
PrivateArk.Init
Set Vault = PrivateArk.AddVaultFromFile("Vault.ini")

' API Logon to Vault Server
Call Logon()
Call DoesUserExist()
Call AddOwners()
Call Logoff()
WScript.Quit
'End Script ---

' Begin Funcitons ---
Function Logon()
	Vault.LogonFromFile("CredFile.ini"),True,False,False
	WScript.Echo "Logged on User: " & Vault.LoggedonUser
End Function

Function DoesUserExist()
	On Error Resume Next
	Set User = Vault.GetUser(Xid)
   		WScript.Echo "User: " & User.Name
   	On Error GoTo 0 
   	If User.Name <> "" Then
   		WScript.Echo "User " & User.Name & " Exists"
   		DoesSafeExist(XID)
   	Else
   		WScript.Echo "User does not exists"	   		
   		WScript.Quit
   	End If
End Function

Function DoesSafeExist(User)
	On Error Resume Next 
	 	SafeName = Vault.GetSafe(User)
	On Error GoTo 0
	
	If SafeName <> "" Then
		WScript.Echo "Safe Name: " & SafeName & " Exists"	
		
		Call AddSafe()
	Else
		Call AddSafe()
	End If
End Function

Function AddSafe()
	' Safe Description
	Set User = Vault.GetUser(Xid)
	Description = User.PersonalDetails.FirstName & " " & User.PersonalDetails.LastName & "'s Safe"
	Vault.AddSafe XID,,,,10,Description,,0,,,,,,,,,,,,,,,SharedAccount,WebAccess,"\Xid",True,,,,,False
End Function

Function AddOwners()
	Set NewSafe = Vault.GetSafe(XID)
	NewSafe.Open
	NewSafe.AddOwner XID, DefaultAccess,,False
	NewSafe.AddOwner "administrator",FullAccess,,False
	NewSafe.Close
End Function

Function Logoff()
	Vault.Logoff
	PrivateArk.Term
	WScript.Echo "Logoff..."
End Function











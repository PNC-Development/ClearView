foreach ($a in $args){

"$a     " + $a.gettype() | Out-File -FilePath C:\Temp\args.txt -Append;
}
throw "Goodbye";
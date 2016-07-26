#Load MySql.Data.dll !!CUSTOMIZE!!
[void][system.reflection.Assembly]::LoadFrom("#####") #Insert path to assambly

#Create variables with custom server data !!CUSTOMIZE!!
$server = "127.0.0.1"
$port = "3307"
$user = "root"
$password = "#######"
$database = "barcode"
$table = "identnummer"
$usedColumn = "identnummer"
$checkColumn = "HasBarcode"

#Create MySQL Objects
$con = New-Object MySql.Data.MySqlClient.MySqlConnection
$con2 = New-Object MySql.Data.MySqlClient.MySqlConnection
$cmd = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd2 = New-Object MySql.Data.MySqlClient.MySqlCommand
$cmd.Connection = $con
$cmd2.Connection = $con2


#Create ConnectionString and CommandText
$con.ConnectionString = [System.String]::Format("server={0};user id={1};password={2};database={3};port={4}",$server,$user,$password,$database,$port)
$con2.ConnectionString = [System.String]::Format("server={0};user id={1};password={2};database={3};port={4}",$server,$user,$password,$database,$port)
$cmd.CommandText = [System.String]::Format("select * from {0} where {1} = 0;",$table,$checkColumn)

try{
#Open Connection and run query
$con.Open()
$con2.Open()
$cmd.ExecuteNonQuery()

#Create MySqlDataReader to read resultset
$rdr = $cmd.ExecuteReader()

#Change to Zint  !!Customize!!
cd '####'  #Insert path to Zint (e.g. C:\Program Files(x84)\Zint)

#Loop trough resultset and create barcode foreach entry
while($rdr.Read()){
    #Read number relevant for barcode creation
    $ident = $rdr.GetInt64(1)
  
    #Create index to name the barcode
    $index = [System.String]::Format("{0}-{1}",$rdr.GetInt16(0),$ident)

    #Create output path !!Customize!!
    $path = [System.String]::Format("###\{0}.eps",$index) #Insert your output directory

    #Run Zint command! See Zint Manual for further information
    .\zint.exe -b 58 -o $path -d $ident

    #Set HasBarcode to 1
    $cmd2.CommandText = [System.String]::Format("update {0} set {1}=1 where {2}={3}",$table, $checkColumn, $usedColumn, $ident)
    $cmd2.ExecuteNonQuery()

}

#Close DataReader and Connection
$rdr.Close()
$con.Close()

Write-Host "Success..."
}
catch{
    Write-Host "An error occured..."
}
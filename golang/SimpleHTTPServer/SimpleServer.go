package main

import (
	"context"
	"database/sql"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"strings"
	"text/template"
	"time"

	_ "github.com/denisenkom/go-mssqldb"
	_ "github.com/go-sql-driver/mysql"
)

var server = "maria-2019"
var port = 1433
var user = "alluser"
var password = "alluser"
var strDataBaseName = "AnalysisDB"

var mysql_server = "127.0.0.1:3306"
var mysql_user = "admin"
var mysql_password = "admin"
var mysql_strDataBaseName = "PNEUMAXDB"

//Public Variable

var strTableName string
var strField string
var strCondition string
var strQuery string
var strExecute string
var CountRecord int

var (
	ctx context.Context
	//db  *sql.DB
	db *sql.DB

	//rows *sql.Rows
	//row *sql.Row
	err error
)
var strSTFCode string

type ListDocument struct {
	DocNo    string `json:"DocNo"`
	CSName   string `json:"CSName"`
	SortDate string `json:"SortDate"`
}

type PartTubeDetail struct {
	Partnid  string `json:"Partnid"`
	Digitno  string `json:"Digitno"`
	Onhand   string `json:"Onhand"`
	DMLength string `json:"DMLength"`
	RealQty  string `json:"RealQty"`
}

type PhysicalCount struct {
	GenNo         string `json:"GenNo"`
	WeekNo        string `json:"WeekNo"`
	IsstimePeriod string `json:"IsstimePeriod"`
	LCcode        string `json:"LCcode"`
	PartNid       string `json:"PartNid"`
}

type PickPartDetail struct {
	TmpTime     string `json:"TmpTime"`
	DocType     string `json:"DocType"`
	DocNo       string `json:"DocNo"`
	PartNID     string `json:"PartNID"`
	LCCode      string `json:"LCCode"`
	Qty         string `json:"Qty"`
	QtyOut      string `json:"QtyOut"`
	QtyDf       string `json:"QtyDf"`
	QtyDfOut    string `json:"QtyDfOut"`
	QtyScrap    string `json:"QtyScrap"`
	QtyScrapOut string `json:"QtyScrapOut"`
	QtyDM       string `json:"QtyDM"`
	QtyDMOut    string `json:"QtyDMOut"`
	DstbQty     string `json:"DstbQty"`
	DstbQtyOut  string `json:"DstbQtyOut"`
	FixDigitNo  string `json:"FixDigitNo"`
	DigitNo     string `json:"DigitNo"`
}

type PickPartJobtubeDetail struct {
	TmpTime     string `json:"TmpTime"`
	DocType     string `json:"DocType"`
	DocNo       string `json:"DocNo"`
	PartNID     string `json:"PartNID"`
	LCCode      string `json:"LCCode"`
	Qty         string `json:"Qty"`
	QtyOut      string `json:"QtyOut"`
	QtyDf       string `json:"QtyDf"`
	QtyDfOut    string `json:"QtyDfOut"`
	QtyScrap    string `json:"QtyScrap"`
	QtyScrapOut string `json:"QtyScrapOut"`
	QtyDM       string `json:"QtyDM"`
	QtyDMOut    string `json:"QtyDMOut"`
	DstbQty     string `json:"DstbQty"`
	DstbQtyOut  string `json:"DstbQtyOut"`
	FixDigitNo  string `json:"FixDigitNo"`
	DigitNo     string `json:"DigitNo"`
}

type ListPart struct {
	PartNid     string `json:"PartNid"`
	Description string `json:"Description"`
}


type ListPartTube struct {
	PartNid     string `json:"PartNid"`
	Digitno     string `json:"Digitno"`
	Description string `json:"Description"`
}

type ListSerialno struct {
	Partnid  string `json:"Partnid"`
	Serialno string `json:"Serialno"`
}

type ProductDetail struct {
	PartNID          string `json:"PartNID"`
	PartNo           string `json:"PartNo"`
	PartDes          string `json:"PartDes"`
	PackQty          string `json:"PackQty"`
	WHcode           string `json:"WHcode"`
	LCcode           string `json:"LCcode"`
	Warehouse        string `json:"Warehouse"`
	LCcode2          string `json:"LCcode2"`
	Warehouse2       string `json:"Warehouse2"`
	PartOnHand       string `json:"PartOnHand"`
	PartOnDefect     string `json:"PartOnDefect"`
	PartOnCRO        string `json:"PartOnCRO"`
	PartOnBOR        string `json:"PartOnBOR"`
	PartOnDamage     string `json:"PartOnDamage"`
	PartOnRSV        string `json:"PartOnRSV"`
	PartOnOrder      string `json:"PartOnOrder"`
	PrintLabel       string `json:"PrintLabel"`
	PartHaveSerialNo string `json:"PartHaveSerialNo"`
}

type ReceivePartDetail struct {
	TmpTime       string `json:"TmpTime"`
	DocType       string `json:"DocType"`
	DocNo         string `json:"DocNo"`
	PartNID       string `json:"PartNID"`
	LCCode        string `json:"LCCode"`
	Qty           string `json:"Qty"`
	QtyIn         string `json:"QtyIn"`
	QtyDf         string `json:"QtyDf"`
	QtyDfIn       string `json:"QtyDfIn"`
	QtyScrap      string `json:"QtyScrap"`
	QtyScrapIn    string `json:"QtyScrapIn"`
	QtyDM         string `json:"QtyDM"`
	QtyDMIn       string `json:"QtyDMIn"`
	PartTubeStock string `json:"PartTubeStock"`
	NumTubeQty    string `json:"NumTubeQty"`
}

type Result struct {
	ResultID      string `json:"ResultID"`
	ResultMessage string `json:"ResultMessage"`
}

type ResultExecuteSQL struct {
	ResultID      string `json:"ResultID"`
	ResultMessage string `json:"ResultMessage"`
}

type ReturnTwoValue struct {
	ResultReturnValue1 string `json:"ResultReturnValue1"`
	ResultReturnValue2 string `json:"ResultReturnValue2"`
}

type ReturnValue struct {
	ResultReturn string `json:"ResultReturn"`
}

type staff struct {
	StfCode                string `json:"STFcode"`
	STFfname               string `json:"STFfname"`
	STFLname               string `json:"STFLname"`
	StfFullName            string `json:"STFfullname"`
	DPcode                 string `json:"DPCode"`
	MobilePickInputManual  string `json:"MobilePickInputManual"`
	MobileStoreInputManual string `json:"MobileStoreInputManual"`
}

type StorePartDetail struct {
	TmpTime       string `json:"TmpTime"`
	DocType       string `json:"DocType"`
	DocNo         string `json:"DocNo"`
	PartNID       string `json:"PartNID"`
	LCCode        string `json:"LCCode"`
	Qty           string `json:"Qty"`
	QtyIn         string `json:"QtyIn"`
	QtyDf         string `json:"QtyDf"`
	QtyDfIn       string `json:"QtyDfIn"`
	QtyScrap      string `json:"QtyScrap"`
	QtyScrapIn    string `json:"QtyScrapIn"`
	QtyDM         string `json:"QtyDM"`
	QtyDMIn       string `json:"QtyDMIn"`
	PartTubeStock string `json:"PartTubeStock"`
	NumTubeQty    string `json:"NumTubeQty"`
}

type StorePartTubeDetail struct {
	TmpTime       string `json:"TmpTime"`
	DocType       string `json:"DocType"`
	DocNo         string `json:"DocNo"`
	PartNID       string `json:"PartNID"`
	LCCode        string `json:"LCCode"`
	Qty           string `json:"Qty"`
	QtyIn         string `json:"QtyIn"`
	QtyDf         string `json:"QtyDf"`
	QtyDfIn       string `json:"QtyDfIn"`
	QtyScrap      string `json:"QtyScrap"`
	QtyScrapIn    string `json:"QtyScrapIn"`
	QtyDM         string `json:"QtyDM"`
	QtyDMIn       string `json:"QtyDMIn"`
	PartTubeStock string `json:"PartTubeStock"`
	NumTubeQty    string `json:"NumTubeQty"`
}

type Tmp_RFPhysicalCount struct {
	TmpTime string `json:"TmpTime"`
	GenNo   string `json:"GenNo"`
	WeekNo  string `json:"WeekNo"`
	PartNID string `json:"PartNID"`
	LCCode  string `json:"LCCode"`
	Qty     string `json:"Qty"`
	Defect  string `json:"Defect"`
	Damage  string `json:"Damage"`
	Scrap   string `json:"Scrap"`
}

type ListDoctype_Receive_Store struct {
	Doctype string `json:"Doctype"`
	Docdesc string `json:"Docdesc"`
}

type ListDocument_Store struct {
	Docno string `json:"Docno"`
	Refno string `json:"Refno"`
}

type ListData2Field struct {
	FieldName1 string `json:"FieldName1"`
	FieldName2 string `json:"FieldName2"`
}
type List_Summary_RFCheckOut struct {
	SelOption string `json:"SelOption"`
	CountDoc  string `json:"CountDoc"`
}

type List_Summary_RFCheckIn struct {
	SelOption string `json:"SelOption"`
	CountDoc  string `json:"CountDoc"`
}

type List_Summary_HandHeldOperate struct {
	SelOption string `json:"SelOption"`
	CountDoc  string `json:"CountDoc"`
}

func main() {

	http.HandleFunc("/", login)
	http.HandleFunc("/Mobile_TestConnectMySql", Mobile_TestConnectMySql)
	http.HandleFunc("/Mobile_CheckLogin_MySql", Mobile_CheckLogin_MySql)
	//==================================================================

	http.HandleFunc("/Mobile_CheckLogin", Mobile_CheckLogin)
	http.HandleFunc("/Mobile_ReturnValue", Mobile_ReturnValue)
	http.HandleFunc("/Mobile_ListDocument", Mobile_ListDocument)
	http.HandleFunc("/Mobile_ConfirmDocument", Mobile_ConfirmDocument)
	http.HandleFunc("/Mobile_DeliveryDocument", Mobile_DeliveryDocument)
	http.HandleFunc("/Mobile_Update_RFCheckOut_PartSerialNo", Mobile_Update_RFCheckOut_PartSerialNo)
	http.HandleFunc("/Mobile_GetSerialNo", Mobile_GetSerialNo)
	http.HandleFunc("/Mobile_GetProductDetail", Mobile_GetProductDetail)
	http.HandleFunc("/Mobile_GetPartTubeDetail", Mobile_GetPartTubeDetail)
	http.HandleFunc("/Mobile_Insert_RFCheckOut_PartSerialNo", Mobile_Insert_RFCheckOut_PartSerialNo)
	http.HandleFunc("/Mobile_Confirm_PickPart", Mobile_Confirm_PickPart)
	http.HandleFunc("/Mobile_Insert_Tmp_RFPartDigitNo", Mobile_Insert_Tmp_RFPartDigitNo)
	http.HandleFunc("/Mobile_GetDoctype", Mobile_GetDoctype)
	http.HandleFunc("/Mobile_CountRecord", Mobile_CountRecord)
	http.HandleFunc("/Mobile_Crt_Tmp_RFCheckOut_Picking", Mobile_Crt_Tmp_RFCheckOut_Picking)
	http.HandleFunc("/Mobile_UPD_Tmp_RFCheckOut", Mobile_UPD_Tmp_RFCheckOut)
	http.HandleFunc("/Mobile_Picking_Hold_Reset", Mobile_Picking_Hold_Reset)
	http.HandleFunc("/Mobile_GetListPart", Mobile_GetListPart)
	http.HandleFunc("/Mobile_GetPickPartDetail", Mobile_GetPickPartDetail)
	http.HandleFunc("/Mobile_GetPhysicalCount", Mobile_GetPhysicalCount)
	http.HandleFunc("/Mobile_Insert_Tmp_RFPhysicalCount", Mobile_Insert_Tmp_RFPhysicalCount)
	http.HandleFunc("/Mobile_Get_Tmp_RFPhysicalCount", Mobile_Get_Tmp_RFPhysicalCount)
	http.HandleFunc("/Mobile_Update_Tmp_RFPhysicalCount", Mobile_Update_Tmp_RFPhysicalCount)
	http.HandleFunc("/Mobile_DeleteRecord", Mobile_DeleteRecord)
	http.HandleFunc("/Mobile_Update_PhysicalCount", Mobile_Update_PhysicalCount)
	http.HandleFunc("/Mobile_SumValue", Mobile_SumValue)
	http.HandleFunc("/Mobile_SqlExecute", Mobile_SqlExecute)
	http.HandleFunc("/Mobile_GetPickPart_JobtubeDetail", Mobile_GetPickPart_JobtubeDetail)
	http.HandleFunc("/Mobile_Confirm_PickPart_Jobtube", Mobile_Confirm_PickPart_Jobtube)
	http.HandleFunc("/Mobile_Insert_Tmp_RFPartTube", Mobile_Insert_Tmp_RFPartTube)
	http.HandleFunc("/Mobile_UPD_Tmp_RFCheckOut_Jobtube", Mobile_UPD_Tmp_RFCheckOut_Jobtube)
	http.HandleFunc("/Mobile_Picking_Jobtube_Hold_Reset", Mobile_Picking_Jobtube_Hold_Reset)
	http.HandleFunc("/Mobile_GetListPartTube", Mobile_GetListPartTube)
	http.HandleFunc("/Mobile_ClearDocNoError", Mobile_ClearDocNoError)
	http.HandleFunc("/Mobile_ListDoctypeReceive", Mobile_ListDoctypeReceive)
	http.HandleFunc("/Mobile_ListDoctypeStore", Mobile_ListDoctypeStore)
	http.HandleFunc("/Mobile_Update_ProductExWH", Mobile_Update_ProductExWH)
	http.HandleFunc("/Mobile_Cancel_ProductExWH", Mobile_Cancel_ProductExWH)
	http.HandleFunc("/Mobile_Update_ProductChecked", Mobile_Update_ProductChecked)
	http.HandleFunc("/Mobile_UPD_Tmp_RFCheckIn", Mobile_UPD_Tmp_RFCheckIn)
	http.HandleFunc("/Mobile_GetReceivePartDetail", Mobile_GetReceivePartDetail)
	http.HandleFunc("/Mobile_Confirm_ReceivePart", Mobile_Confirm_ReceivePart)
	http.HandleFunc("/Mobile_INS_Tmp_RFCheckIn_ChkDigit", Mobile_INS_Tmp_RFCheckIn_ChkDigit)
	http.HandleFunc("/Mobile_INS_Tmp_RFCheckIn", Mobile_INS_Tmp_RFCheckIn)
	http.HandleFunc("/Mobile_GetStorePartDetail", Mobile_GetStorePartDetail)
	http.HandleFunc("/Mobile_Confirm_StorePart", Mobile_Confirm_StorePart)
	http.HandleFunc("/Mobile_Store_Hold_Reset", Mobile_Store_Hold_Reset)
	http.HandleFunc("/Mobile_ListDocument_Store", Mobile_ListDocument_Store)
	http.HandleFunc("/Mobile_ListDocument_Receive", Mobile_ListDocument_Receive)
	http.HandleFunc("/Mobile_GetServerAddress", Mobile_GetServerAddress)
	http.HandleFunc("/Mobile_GetListData2Field", Mobile_GetListData2Field)
	http.HandleFunc("/Mobile_SummaryRFCheckOut", Mobile_SummaryRFCheckOut)
	http.HandleFunc("/Mobile_SummaryRFCheckIn", Mobile_SummaryRFCheckIn)
	http.HandleFunc("/Mobile_SummaryHandHeldOperate", Mobile_SummaryHandHeldOperate)
	http.HandleFunc("/Mobile_GetDataSpinner", Mobile_GetDataSpinner)

	//Test
	http.HandleFunc("/ExampleTx_Rollback", ExampleTx_Rollback)
	http.HandleFunc("/ExampleDB_BeginTx", ExampleDB_BeginTx)
	http.HandleFunc("/ExampleDB_ExecContext", ExampleDB_ExecContext)
	http.HandleFunc("/ExampleDB_Query_multipleResultSets", ExampleDB_Query_multipleResultSets)
	http.HandleFunc("/ExampleDB_PingContext", ExampleDB_PingContext)
	http.HandleFunc("/ExampleDB_Prepare", ExampleDB_Prepare)
	http.HandleFunc("/ExampleTx_Prepare", ExampleTx_Prepare)
	http.HandleFunc("/ExampleStmt", ExampleStmt)

	http.ListenAndServe(":8080", nil)
}

//======================== Start Test MySql ===================================
func ConnectServerMySql(database string) {
	connString := fmt.Sprintf("%s:%s@tcp(%s)/%s", mysql_user, mysql_password, mysql_server, mysql_strDataBaseName)
	db, err = sql.Open("mysql", connString)
	if err != nil {
		log.Fatalf("Error Creating connection pool:" + err.Error())
	}
}

func Mobile_TestConnectMySql(w http.ResponseWriter, r *http.Request) {
	ConnectServerMySql(strDataBaseName)
	defer db.Close()
	rows, err := db.Query("select stfcode,stfFullname from staff where stfcode='1233'")
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	for rows.Next() {
		var strStfCode string
		var strStfFullname string
		err = rows.Scan(&strStfCode, &strStfFullname)
		fmt.Fprintf(w, "StfCode : %s \nName:%s \n", strStfCode, strStfFullname)
	}
}

func Mobile_CheckLogin_MySql(w http.ResponseWriter, r *http.Request) {
	var strUserID string
	var strPassword string
	var result staff
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strUserID = r.FormValue("strUserID")
	strPassword = r.FormValue("strPassword")

	fmt.Fprintf(w, "Test")

	ConnectServerMySql(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	err = db.QueryRowContext(ctx, "CALL Mobile_CheckLogin ('?','?')", strUserID, strPassword).
		Scan(&result.StfCode, &result.STFfname, &result.STFLname, &result.StfFullName, &result.DPcode,
			&result.MobilePickInputManual, &result.MobileStoreInputManual)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		staff := &staff{StfCode: result.StfCode, STFfname: result.STFfname, STFLname: result.STFLname,
			StfFullName: result.StfFullName, DPcode: result.DPcode, MobilePickInputManual: result.MobilePickInputManual,
			MobileStoreInputManual: result.MobileStoreInputManual}
		e, err := json.Marshal(staff)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

//======================== End Test MySql ===================================

func ConnectServer(database string) {
	connString := fmt.Sprintf("server=%s;user id=%s;password=%s;port=%d;database=%s;", server, user, password, port, database)
	db, err = sql.Open("sqlserver", connString)
	if err != nil {
		log.Fatalf("Error Creating connection pool:" + err.Error())
	}
}

func Mobile_GetDataSpinner(w http.ResponseWriter, r *http.Request) {
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select Distinct " + strField + " From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}

	rows, err := db.QueryContext(ctx, strQuery)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ReturnValue
		if err := rows.Scan(&result.ResultReturn); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_SummaryHandHeldOperate(w http.ResponseWriter, r *http.Request) {
	var strOption string
	var strWorkType string
	var strDeviceName string
	var strFromDate string
	var strToDate string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strOption = r.FormValue("strOption")
	strWorkType = r.FormValue("strWorkType")
	strDeviceName = r.FormValue("strDeviceName")
	strFromDate = r.FormValue("strFromDate")
	strToDate = r.FormValue("strToDate")
	// เช็คข้อมูลจาก พารามิเตอร์
	//fmt.Fprintf(w,"Paramete Value %s %s %s %s %s",strDataBaseName,strOption,strWorkType,strDeviceName,strFromDate,strToDate)

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	rows, err := db.QueryContext(ctx, "exec Mobile_SummaryHandHeldOperate @p1,@p2,@p3,@p4,@p5", strOption, strWorkType, strDeviceName, strFromDate, strToDate)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	//fmt.Fprintf(w,"Step for rowsNext")

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result List_Summary_HandHeldOperate
		if err := rows.Scan(&result.SelOption, &result.CountDoc); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))
	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}

}

func Mobile_SummaryRFCheckIn(w http.ResponseWriter, r *http.Request) {
	var strOption string
	var strWorkType string
	var strStfcode string
	var strFromDate string
	var strToDate string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strOption = r.FormValue("strOption")
	strWorkType = r.FormValue("strWorkType")
	strStfcode = r.FormValue("strStfcode")
	strFromDate = r.FormValue("strFromDate")
	strToDate = r.FormValue("strToDate")
	// เช็คข้อมูลจาก พารามิเตอร์
	//fmt.Fprintf(w,"Paramete Value %s %s %s %s %s",strDataBaseName,strOption,strWorkType,strStfcode,strFromDate,strToDate)

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	rows, err := db.QueryContext(ctx, "exec Mobile_SummaryRFCheckIn @p1,@p2,@p3,@p4,@p5", strOption, strWorkType, strStfcode, strFromDate, strToDate)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	//fmt.Fprintf(w,"Step for rowsNext")

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result List_Summary_RFCheckIn
		if err := rows.Scan(&result.SelOption, &result.CountDoc); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))
	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_SummaryRFCheckOut(w http.ResponseWriter, r *http.Request) {
	var strOption string
	var strWorkType string
	var strStfcode string
	var strFromDate string
	var strToDate string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strOption = r.FormValue("strOption")
	strWorkType = r.FormValue("strWorkType")
	strStfcode = r.FormValue("strStfcode")
	strFromDate = r.FormValue("strFromDate")
	strToDate = r.FormValue("strToDate")
	// เช็คข้อมูลจาก พารามิเตอร์
	//fmt.Fprintf(w,"Paramete Value %s %s %s %s %s",strDataBaseName,strOption,strWorkType,strStfcode,strFromDate,strToDate)

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	rows, err := db.QueryContext(ctx, "exec Mobile_SummaryRFCheckOut @p1,@p2,@p3,@p4,@p5", strOption, strWorkType, strStfcode, strFromDate, strToDate)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	//fmt.Fprintf(w,"Step for rowsNext")

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result List_Summary_RFCheckOut
		if err := rows.Scan(&result.SelOption, &result.CountDoc); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))
	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}

}

func Mobile_GetListData2Field(w http.ResponseWriter, r *http.Request) {

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select Distinct " + strField + " From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}
	rows, err := db.QueryContext(ctx, strQuery)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}

	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListData2Field
		if err := rows.Scan(&result.FieldName1, &result.FieldName2); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))
	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}

}

func Mobile_GetServerAddress(w http.ResponseWriter, r *http.Request) {
	var result ReturnValue

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select Distinct " + strField + " From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}
	err = db.QueryRowContext(ctx, strQuery).
		Scan(&result.ResultReturn)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]") //ถ้าไม่มีข้อมูลให้ส่งเป็นว่างกลับไป
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		ReturnValue := &ReturnValue{ResultReturn: result.ResultReturn}
		e, err := json.Marshal(ReturnValue)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_ListDocument_Receive(w http.ResponseWriter, r *http.Request) {
	var strDoctype string
	var strStfcode string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDoctype = r.FormValue("strDoctype")
	strStfcode = r.FormValue("strStfcode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_ListDocument_Receive @p1,@p2", strDoctype, strStfcode)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListDocument_Store
		if err := rows.Scan(&result.Docno, &result.Refno); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_ListDocument_Store(w http.ResponseWriter, r *http.Request) {

	var strDoctype string
	var strStfcode string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDoctype = r.FormValue("strDoctype")
	strStfcode = r.FormValue("strStfcode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_ListDocument_Store @p1,@p2", strDoctype, strStfcode)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListDocument_Store
		if err := rows.Scan(&result.Docno, &result.Refno); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_Store_Hold_Reset(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strPartnid string
	var strOption string
	var strStfCode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strPartnid = r.FormValue("strPartnid")
	strOption = r.FormValue("strOption")
	strStfCode = r.FormValue("strStfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Store_Hold_Reset @p1,@p2,@p3,@p4,@p5",
		strDocNo, strPartnid, strOption, strStfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Confirm_StorePart(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strDocNo string
	var strPartNid string
	var strStfcode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strDocNo = r.FormValue("strDocNo")
	strPartNid = r.FormValue("strPartNid")
	strStfcode = r.FormValue("strStfcode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Confirm_StorePart @p1,@p2,@p3,@p4,@p5",
		strTmpTime, strDocNo, strPartNid, strStfcode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_GetStorePartDetail(w http.ResponseWriter, r *http.Request) {
	var strPartnid string
	var strDocno string
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strPartnid = r.FormValue("strPartnid")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_GetStorePartDetail @p1,@p2", strDocno, strPartnid)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ReceivePartDetail
		if err := rows.Scan(&result.TmpTime, &result.DocType, &result.DocNo,
			&result.PartNID, &result.LCCode, &result.Qty, &result.QtyIn,
			&result.QtyDf, &result.QtyDfIn, &result.QtyScrap, &result.QtyScrapIn,
			&result.QtyDM, &result.QtyDMIn,
			&result.PartTubeStock, &result.NumTubeQty); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_INS_Tmp_RFCheckIn(w http.ResponseWriter, r *http.Request) {
	var strDocno string
	var strStfCode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strStfCode = r.FormValue("strStfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_INS_Tmp_RFCheckIn @p1,@p2,@p3",
		strDocno, strStfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_INS_Tmp_RFCheckIn_ChkDigit(w http.ResponseWriter, r *http.Request) {
	var strDocno string
	var strStfcode string
	var strPartnid string
	var strParttubestock string
	var strKind string
	var strDigit string
	var strQty string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strStfcode = r.FormValue("strStfcode")
	strPartnid = r.FormValue("strPartnid")
	strParttubestock = r.FormValue("strParttubestock")
	strKind = r.FormValue("strKind")
	strDigit = r.FormValue("strDigit")
	strQty = r.FormValue("strQty")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_INS_Tmp_RFCheckIn_ChkDigit @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8",
		strDocno, strStfcode, strPartnid, strParttubestock, strKind, strDigit, strQty, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Confirm_ReceivePart(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strDocNo string
	var strPartNid string
	var strStfcode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strDocNo = r.FormValue("strDocNo")
	strPartNid = r.FormValue("strPartNid")
	strStfcode = r.FormValue("strStfcode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Confirm_ReceivePart @p1,@p2,@p3,@p4,@p5",
		strTmpTime, strDocNo, strPartNid, strStfcode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_GetReceivePartDetail(w http.ResponseWriter, r *http.Request) {
	var strPartnid string
	var strDocno string
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strPartnid = r.FormValue("strPartnid")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_GetReceivePartDetail @p1,@p2", strDocno, strPartnid)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ReceivePartDetail
		if err := rows.Scan(&result.TmpTime, &result.DocType, &result.DocNo,
			&result.PartNID, &result.LCCode, &result.Qty, &result.QtyIn,
			&result.QtyDf, &result.QtyDfIn, &result.QtyScrap, &result.QtyScrapIn,
			&result.QtyDM, &result.QtyDMIn,
			&result.PartTubeStock, &result.NumTubeQty); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_UPD_Tmp_RFCheckIn(w http.ResponseWriter, r *http.Request) {
	var strDocno string
	var strPartnid string
	var strKind string
	var strDigit string
	var strQty string
	var strStatus string
	var strSTFcode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strPartnid = r.FormValue("strPartnid")
	strKind = r.FormValue("strKind")
	strDigit = r.FormValue("strDigit")
	strQty = r.FormValue("strQty")
	strStatus = r.FormValue("strStatus")
	strSTFcode = r.FormValue("strSTFcode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_UPD_Tmp_RFCheckIn @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8",
		strDocno, strPartnid, strKind, strDigit, strQty, strStatus, strSTFcode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Update_ProductChecked(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strStfCode string
	var strOption string
	var strPartnid string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strStfCode = r.FormValue("strStfCode")
	strOption = r.FormValue("strOption")
	strPartnid = r.FormValue("strPartnid")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Update_ProductChecked @p1,@p2,@p3,@p4,@p5", strDocNo, strStfCode, strOption, strPartnid, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Cancel_ProductExWH(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strStfCode string
	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strStfCode = r.FormValue("strStfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Cancel_ProductExWH @p1,@p2,@p3", strDocNo, strStfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Update_ProductExWH(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strStfCode string
	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strStfCode = r.FormValue("strStfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Update_ProductExWH @p1,@p2,@p3", strDocNo, strStfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_ListDoctypeStore(w http.ResponseWriter, r *http.Request) {

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_ListDoctypeStore")
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListDoctype_Receive_Store
		if err := rows.Scan(&result.Doctype, &result.Docdesc); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_ListDoctypeReceive(w http.ResponseWriter, r *http.Request) {

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_ListDoctypeReceive")
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListDoctype_Receive_Store
		if err := rows.Scan(&result.Doctype, &result.Docdesc); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_ClearDocNoError(w http.ResponseWriter, r *http.Request) {
	var strOptionType string
	var strOptionClear string
	var strDocNo string
	var strStfCode string
	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strOptionType = r.FormValue("strOptionType")
	strOptionClear = r.FormValue("strOptionClear")
	strStfCode = r.FormValue("strStfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_ClearDocNoError @p1,@p2,@p3,@p4,@p5",
		strDocNo, strOptionType, strOptionClear, strStfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_GetListPartTube(w http.ResponseWriter, r *http.Request) {

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select Distinct " + strField + " From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}
	rows, err := db.QueryContext(ctx, strQuery)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}

	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListPartTube
		if err := rows.Scan(&result.PartNid, &result.Digitno, &result.Description); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))
	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}

}

func Mobile_Picking_Jobtube_Hold_Reset(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strPartnid string
	var strOption string
	var strStfCode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strPartnid = r.FormValue("strPartnid")
	strOption = r.FormValue("strOption")
	strStfCode = r.FormValue("strStfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Picking_Jobtube_Hold_Reset @p1,@p2,@p3,@p4,@p5", strDocNo, strPartnid, strOption, strStfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_UPD_Tmp_RFCheckOut_Jobtube(w http.ResponseWriter, r *http.Request) {
	var strDocno string
	var strPartnid string
	var strDigitno string
	var strQty string
	var strDstbQty string
	var strKind string
	var strQtyCut string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strPartnid = r.FormValue("strPartnid")
	strDigitno = r.FormValue("strDigitno")
	strQty = r.FormValue("strQty")
	strDstbQty = r.FormValue("strDstbQty")
	strKind = r.FormValue("strKind")
	strQtyCut = r.FormValue("strQtyCut")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_UPD_Tmp_RFCheckOut @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8",
		strDocno, strPartnid, strDigitno, strQty, strDstbQty, strKind, strQtyCut, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Insert_Tmp_RFPartTube(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strPartNid string
	var strDigitNo string
	var strQty string
	var strDstbQty string
	var strDigitNoCut string
	var strCutLength string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strPartNid = r.FormValue("strPartNid")
	strDigitNo = r.FormValue("strDigitNo")
	strQty = r.FormValue("strQty")
	strDstbQty = r.FormValue("strDstbQty")
	strDigitNoCut = r.FormValue("strDigitNoCut")
	strCutLength = r.FormValue("strCutLength")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Insert_Tmp_RFPartTube @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8",
		strDocNo, strPartNid, strDigitNo, strQty, strDstbQty, strDigitNoCut, strCutLength, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Confirm_PickPart_Jobtube(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strDocNo string
	var strPartNid string
	var strDigitno string
	var strQty string
	var strDstbQty string
	var strStfcode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strDocNo = r.FormValue("strDocNo")
	strPartNid = r.FormValue("strPartNid")
	strDigitno = r.FormValue("strDigitno")
	strQty = r.FormValue("strQty")
	strDstbQty = r.FormValue("strDstbQty")
	strStfcode = r.FormValue("strStfcode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Confirm_PickPart_Jobtube @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8",
		strTmpTime, strDocNo, strPartNid, strDigitno, strQty, strDstbQty, strStfcode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_GetPickPart_JobtubeDetail(w http.ResponseWriter, r *http.Request) {
	var strPickTubeDocno string
	var strPickTubePartnid string
	var strPickTubeDigitno string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strPickTubeDocno = r.FormValue("strDocno")
	strPickTubePartnid = r.FormValue("strPartnid")
	strPickTubeDigitno = r.FormValue("strDigitno")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_GetPickPart_JobtubeDetail @p1,@p2,@p3", strPickTubeDocno, strPickTubePartnid, strPickTubeDigitno)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result PickPartJobtubeDetail
		if err := rows.Scan(&result.TmpTime, &result.DocType, &result.DocNo, &result.PartNID, &result.LCCode,
			&result.Qty, &result.QtyOut, &result.QtyDf, &result.QtyDfOut, &result.QtyScrap, &result.QtyScrapOut,
			&result.QtyDM, &result.QtyDMOut, &result.DstbQty, &result.DstbQtyOut, &result.FixDigitNo, &result.DigitNo); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_SqlExecute(w http.ResponseWriter, r *http.Request) {

	var result Result

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strExecute = r.FormValue("strForExecute")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	tx, err := db.BeginTx(ctx, &sql.TxOptions{Isolation: sql.LevelSerializable})

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	_, execErr := tx.Exec(strExecute)
	if execErr != nil {
		_ = tx.Rollback()
		result.ResultID = "UnSuccess"
		result.ResultMessage = strExecute
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
		return
	}
	//ถ้า  err != nil แสดงว่ามี Error
	if err := tx.Commit(); err != nil {
		result.ResultID = "UnSuccess"
		result.ResultMessage = strExecute
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
		return
	}

	result.ResultID = "Success"
	result.ResultMessage = strExecute
	Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
	e, err := json.Marshal(Result)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
		return
	}
	fmt.Fprintf(w, "%s", string(e))

}

func Mobile_SumValue(w http.ResponseWriter, r *http.Request) {
	var result ReturnValue
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select isnull(Sum(" + strField + "),0) as ResultReturn From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}
	err = db.QueryRowContext(ctx, strQuery).
		Scan(&result.ResultReturn)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		ReturnValue := &ReturnValue{ResultReturn: result.ResultReturn}
		e, err := json.Marshal(ReturnValue)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Update_PhysicalCount(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strPartNid string
	var strQty string
	var strDF string
	var strSDM string
	var strDM string
	var strCardQty string
	var strCardDF string
	var strCardSDM string
	var strCardDM string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strPartNid = r.FormValue("strPartNid")
	strQty = r.FormValue("strQty")
	strDF = r.FormValue("strDF")
	strSDM = r.FormValue("strSDM")
	strDM = r.FormValue("strDM")
	strCardQty = r.FormValue("strCardQty")
	strCardDF = r.FormValue("strCardDF")
	strCardSDM = r.FormValue("strCardSDM")
	strCardDM = r.FormValue("strCardDM")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Update_PhysicalCount @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11",
		strTmpTime, strPartNid, strQty, strDF, strSDM, strDM, strCardQty, strCardDF, strCardSDM, strCardDM, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_DeleteRecord(w http.ResponseWriter, r *http.Request) {

	var result Result

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	tx, err := db.BeginTx(ctx, &sql.TxOptions{Isolation: sql.LevelSerializable})

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	strExecute = "Delete From " + strTableName + " where " + strCondition
	if strings.ToUpper(strTableName) == "TMP_RFPHYSICALCOUNT" {
		strExecute = strExecute + "; Delete From Tmp_RFPartDigitNo where " + strCondition
	}

	_, execErr := tx.Exec(strExecute)
	if execErr != nil {
		_ = tx.Rollback()
		result.ResultID = "UnSuccess"
		result.ResultMessage = strExecute
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
		return
	}
	//ถ้า  err != nil แสดงว่ามี Error
	if err := tx.Commit(); err != nil {
		result.ResultID = "UnSuccess"
		result.ResultMessage = strExecute
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
		return
	}

	result.ResultID = "Success"
	result.ResultMessage = strExecute
	Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
	e, err := json.Marshal(Result)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
		return
	}
	fmt.Fprintf(w, "%s", string(e))

}

func Mobile_Update_Tmp_RFPhysicalCount(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strPartNid string
	var strQty string
	var strDF string
	var strSDM string
	var strDM string
	var strCardQty string
	var strCardDF string
	var strCardSDM string
	var strCardDM string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strPartNid = r.FormValue("strPartNid")
	strQty = r.FormValue("strQty")
	strDF = r.FormValue("strDF")
	strSDM = r.FormValue("strSDM")
	strDM = r.FormValue("strDM")
	strCardQty = r.FormValue("strCardQty")
	strCardDF = r.FormValue("strCardDF")
	strCardSDM = r.FormValue("strCardSDM")
	strCardDM = r.FormValue("strCardDM")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Update_Tmp_RFPhysicalCount @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11",
		strTmpTime, strPartNid, strQty, strDF, strSDM, strDM, strCardQty, strCardDF, strCardSDM, strCardDM, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Get_Tmp_RFPhysicalCount(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strSTfCode string
	var strGenNo string
	var strWeekNo string
	var strLCCode string
	var strPartNid string

	var result Tmp_RFPhysicalCount

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strSTfCode = r.FormValue("strSTfCode")
	strGenNo = r.FormValue("strGenNo")
	strWeekNo = r.FormValue("strWeekNo")
	strLCCode = r.FormValue("strLCCode")
	strPartNid = r.FormValue("strPartNid")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Get_Tmp_RFPhysicalCount @p1,@p2,@p3,@p4,@p5,@p6,@p7",
		strTmpTime, strSTfCode, strGenNo, strWeekNo, strLCCode, strPartNid, "").
		Scan(&result.TmpTime, &result.GenNo, &result.WeekNo, &result.PartNID, &result.LCCode,
			&result.Qty, &result.Defect, &result.Damage, &result.Scrap)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Tmp_RFPhysicalCount{TmpTime: result.TmpTime, GenNo: result.GenNo, WeekNo: result.WeekNo,
			PartNID: result.PartNID, LCCode: result.LCCode, Qty: result.Qty,
			Defect: result.Defect, Damage: result.Damage, Scrap: result.Scrap}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Insert_Tmp_RFPhysicalCount(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strSTfCode string
	var strGenNo string
	var strWeekNo string
	var strLCCode string
	var strPartNid string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strSTfCode = r.FormValue("strSTfCode")
	strGenNo = r.FormValue("strGenNo")
	strWeekNo = r.FormValue("strWeekNo")
	strLCCode = r.FormValue("strLCCode")
	strPartNid = r.FormValue("strPartNid")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Insert_Tmp_RFPhysicalCount @p1,@p2,@p3,@p4,@p5,@p6,@p7",
		strTmpTime, strSTfCode, strGenNo, strWeekNo, strLCCode, strPartNid, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_GetPhysicalCount(w http.ResponseWriter, r *http.Request) {
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select Distinct " + strField + " From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}

	rows, err := db.QueryContext(ctx, strQuery)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ReturnValue
		if err := rows.Scan(&result.ResultReturn); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_GetPickPartDetail(w http.ResponseWriter, r *http.Request) {
	var strDocno string
	var strPartnid string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strPartnid = r.FormValue("strPartnid")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_GetPickPartDetail @p1,@p2", strDocno, strPartnid)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result PickPartDetail
		if err := rows.Scan(&result.TmpTime, &result.DocType, &result.DocNo, &result.PartNID, &result.LCCode,
			&result.Qty, &result.QtyOut, &result.QtyDf, &result.QtyDfOut, &result.QtyScrap, &result.QtyScrapOut,
			&result.QtyDM, &result.QtyDMOut, &result.DstbQty, &result.DstbQtyOut, &result.FixDigitNo, &result.DigitNo); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_GetListPart(w http.ResponseWriter, r *http.Request) {

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select Distinct " + strField + " From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}
	rows, err := db.QueryContext(ctx, strQuery)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}

	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListPart
		if err := rows.Scan(&result.PartNid, &result.Description); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))
	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}

}

func Mobile_Picking_Hold_Reset(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strPartnid string
	var strOption string
	var strStfCode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strPartnid = r.FormValue("strPartnid")
	strOption = r.FormValue("strOption")
	strStfCode = r.FormValue("strStfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Picking_Hold_Reset @p1,@p2,@p3,@p4,@p5", strDocNo, strPartnid, strOption, strStfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_UPD_Tmp_RFCheckOut(w http.ResponseWriter, r *http.Request) {
	var strDocno string
	var strPartnid string
	var strKind string
	var strQty string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strPartnid = r.FormValue("strPartnid")
	strKind = r.FormValue("strKind")
	strQty = r.FormValue("strQty")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_UPD_Tmp_RFCheckOut @p1,@p2,@p3,@p4,@p5", strDocno, strPartnid, strKind, strQty, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Crt_Tmp_RFCheckOut_Picking(w http.ResponseWriter, r *http.Request) {
	var strDocno string
	var strSTfCode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")
	strSTfCode = r.FormValue("strSTfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Crt_Tmp_RFCheckOut_Picking @p1,@p2,@p3", strDocno, strSTfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_CountRecord(w http.ResponseWriter, r *http.Request) {
	var result ReturnValue
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select count(*) as ResultReturn From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}
	err = db.QueryRowContext(ctx, strQuery).
		Scan(&result.ResultReturn)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		ReturnValue := &ReturnValue{ResultReturn: result.ResultReturn}
		e, err := json.Marshal(ReturnValue)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_GetDoctype(w http.ResponseWriter, r *http.Request) {
	var strDocno string
	var result ReturnValue
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocno = r.FormValue("strDocno")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_GetDoctype @p1", strDocno).
		Scan(&result.ResultReturn)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		ReturnValue := &ReturnValue{ResultReturn: result.ResultReturn}
		e, err := json.Marshal(ReturnValue)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Insert_Tmp_RFPartDigitNo(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strDocNo string
	var strPartDigitNo string
	var strPartNid string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strDocNo = r.FormValue("strDocNo")
	strPartDigitNo = r.FormValue("strPartDigitNo")
	strPartNid = r.FormValue("strPartNid")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Insert_Tmp_RFPartDigitNo @p1,@p2,@p3,@p4,@p5", strTmpTime, strDocNo, strPartDigitNo, strPartNid, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Confirm_PickPart(w http.ResponseWriter, r *http.Request) {
	var strTmpTime string
	var strDocNo string
	var strPartNid string
	var strStfcode string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTmpTime = r.FormValue("strTmpTime")
	strDocNo = r.FormValue("strDocNo")
	strPartNid = r.FormValue("strPartNid")
	strStfcode = r.FormValue("strStfcode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Confirm_PickPart @p1,@p2,@p3,@p4,@p5", strTmpTime, strDocNo, strPartNid, strStfcode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_Insert_RFCheckOut_PartSerialNo(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strPartnid string
	var strSerialno string

	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strPartnid = r.FormValue("strPartnid")
	strSerialno = r.FormValue("strSerialno")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Insert_RFCheckOut_PartSerialNo @p1,@p2,@p3,@p4", strDocNo, strPartnid, strSerialno, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_GetPartTubeDetail(w http.ResponseWriter, r *http.Request) {
	var strPartnid string
	var strDigitno string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strPartnid = r.FormValue("strPartnid")
	strDigitno = r.FormValue("strDigitno")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_GetPartTubeDetail @p1,@p2", strPartnid, strDigitno)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result PartTubeDetail
		if err := rows.Scan(&result.Partnid, &result.Digitno, &result.Onhand,
			&result.DMLength, &result.RealQty); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_GetProductDetail(w http.ResponseWriter, r *http.Request) {
	var strPartnid string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strPartnid = r.FormValue("strPartnid")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_GetProductDetail @p1", strPartnid)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ProductDetail
		if err := rows.Scan(&result.PartNID, &result.PartNo, &result.PartDes,
			&result.PackQty, &result.WHcode, &result.LCcode, &result.Warehouse,
			&result.LCcode2, &result.Warehouse2, &result.PartOnHand,
			&result.PartOnDefect, &result.PartOnCRO, &result.PartOnBOR,
			&result.PartOnDamage, &result.PartOnRSV, &result.PartOnOrder,
			&result.PrintLabel, &result.PartHaveSerialNo); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_GetSerialNo(w http.ResponseWriter, r *http.Request) {
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select Distinct " + strField + " From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}

	rows, err := db.QueryContext(ctx, strQuery)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListSerialno
		if err := rows.Scan(&result.Partnid, &result.Serialno); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_Update_RFCheckOut_PartSerialNo(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strSerialno string
	var strDeliveryCode string
	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strSerialno = r.FormValue("strSerialno")
	strDeliveryCode = r.FormValue("strDeliveryCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_Update_RFCheckOut_PartSerialNo @p1,@p2,@p3", strDocNo, strSerialno, strDeliveryCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_DeliveryDocument(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strDeliveryCode string
	var strReceiveCode string
	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strDeliveryCode = r.FormValue("strDeliveryCode")
	strReceiveCode = r.FormValue("strReceiveCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_DeliveryDocument @p1,@p2,@p3,@p4", strDocNo, strDeliveryCode, strReceiveCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_ConfirmDocument(w http.ResponseWriter, r *http.Request) {
	var strDocNo string
	var strStfCode string
	var result Result
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strDocNo = r.FormValue("strDocNo")
	strStfCode = r.FormValue("strStfCode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_ConfirmDocument @p1,@p2,@p3", strDocNo, strStfCode, "").
		Scan(&result.ResultID, &result.ResultMessage)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		Result := &Result{ResultID: result.ResultID, ResultMessage: result.ResultMessage}
		e, err := json.Marshal(Result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func Mobile_ListDocument(w http.ResponseWriter, r *http.Request) {
	var strOption string
	var strStfcode string

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strOption = r.FormValue("strOption")
	strStfcode = r.FormValue("strStfcode")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	rows, err := db.QueryContext(ctx, "exec Mobile_ListDocument @p1,@p2", strOption, strStfcode)
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result ListDocument
		if err := rows.Scan(&result.DocNo, &result.CSName, &result.SortDate); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			//fmt.Fprintf(w,"Error %s",err)
			fmt.Fprintf(w, "%s", (err))
			return
		}
		fmt.Fprintf(w, "%s,", (string(e)))

	}
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func Mobile_ReturnValue(w http.ResponseWriter, r *http.Request) {
	var result ReturnValue

	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strTableName = r.FormValue("strTableName")
	strField = r.FormValue("strField")
	strCondition = r.FormValue("strCondition")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	strQuery = "Select Distinct " + strField + " From " + strTableName
	if strCondition != "" {
		strQuery = strQuery + " where " + strCondition
	}
	err = db.QueryRowContext(ctx, strQuery).
		Scan(&result.ResultReturn)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]") //ถ้าไม่มีข้อมูลให้ส่งเป็นว่างกลับไป
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		ReturnValue := &ReturnValue{ResultReturn: result.ResultReturn}
		e, err := json.Marshal(ReturnValue)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}


func Mobile_CheckLogin(w http.ResponseWriter, r *http.Request) {
	var strUserID string
	var strPassword string
	var result staff
	r.ParseForm()
	strDataBaseName = r.FormValue("strDataBaseName")
	strUserID = r.FormValue("strUserID")
	strPassword = r.FormValue("strPassword")

	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}
	err = db.QueryRowContext(ctx, "exec Mobile_CheckLogin @p1,@p2", strUserID, strPassword).
		Scan(&result.StfCode, &result.STFfname, &result.STFLname, &result.StfFullName, &result.DPcode,
			&result.MobilePickInputManual, &result.MobileStoreInputManual)

	switch {
	case err == sql.ErrNoRows:
		fmt.Fprintf(w, "[]")
	case err != nil:
		fmt.Fprintf(w, "query error: %v\n", err)
	default:
		staff := &staff{StfCode: result.StfCode, STFfname: result.STFfname, STFLname: result.STFLname,
			StfFullName: result.StfFullName, DPcode: result.DPcode, MobilePickInputManual: result.MobilePickInputManual,
			MobileStoreInputManual: result.MobileStoreInputManual}
		e, err := json.Marshal(staff)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		fmt.Fprintf(w, "%s", string(e))
	}
}

func login(w http.ResponseWriter, r *http.Request) {
	fmt.Println("method:", r.Method) //get request method
	if r.Method == "GET" {
		t, _ := template.ParseFiles("login.gtpl")
		t.Execute(w, nil)
	} else {
		r.ParseForm()
		strSTFCode = r.FormValue("STFCode")
		ExampleDB_QueryContext(w, r)
	}
}

//========================================================================
//ทดสอบ โปรแกรม

//อันน่าใช้ดี ถ้าทำงานไม่สำเร็จจะ Rollback ให้ สามารถทำงานได้หลาย Statement
func ExampleTx_Rollback(w http.ResponseWriter, r *http.Request) {
	strDataBaseName = "AnalysisDB"
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	tx, err := db.BeginTx(ctx, &sql.TxOptions{Isolation: sql.LevelSerializable})

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	_, err = tx.ExecContext(ctx, "INSERT INTO projects(id, mascot, release, category) VALUES( @p1, @p2, @p3, @p4)", 4, "paid", 200, "OPen")

	if err != nil {
		if rollbackErr := tx.Rollback(); rollbackErr != nil {
			fmt.Fprintf(w, "update projects: unable to rollback: %v", rollbackErr)
		}
		fmt.Fprintf(w, "Error %s", err)
	}

	id := 4

	_, err = tx.ExecContext(ctx, "UPDATE projects SET mascot = @p1 WHERE id = @p2", "XXX", id)

	if err != nil {
		if rollbackErr := tx.Rollback(); rollbackErr != nil {
			fmt.Fprintf(w, "update projects: unable to rollback: %v", rollbackErr)
		}
		fmt.Fprintf(w, "Error %s", err)
	}
	id1 := 1

	_, err = tx.ExecContext(ctx, "UPDATE projects SET mascot = @p1 WHERE id = @p2", "paid", id1)

	if err != nil {

		if rollbackErr := tx.Rollback(); rollbackErr != nil {
			fmt.Fprintf(w, "update failed: %v, unable to back: %v", err, rollbackErr)
		}

		fmt.Fprintf(w, "Error %s", err)

	}

	if err := tx.Commit(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	} else {
		fmt.Fprintf(w, "Complete")
	}

}

func ExampleDB_BeginTx(w http.ResponseWriter, r *http.Request) {
	strDataBaseName = "AnalysisDB"
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	tx, err := db.BeginTx(ctx, &sql.TxOptions{Isolation: sql.LevelSerializable})

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	id := 4

	_, execErr := tx.Exec("Delete From projects Where id=3 ;UPDATE projects SET mscot = @p1 WHERE id = @p2", "paid", id)

	if execErr != nil {

		_ = tx.Rollback()

		fmt.Fprintf(w, "Error %s", execErr)

	}

	if err := tx.Commit(); err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

}

//ดึงข้อมูลใส่ Rows
func ExampleDB_QueryContext(w http.ResponseWriter, r *http.Request) {
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	rows, err := db.QueryContext(ctx, "SELECT rtrim(stfcode),rtrim(StfFullname),DPcode FROM Staff WHERE DPCode=@p1", "MIS")
	if err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var result staff
		if err := rows.Scan(&result.StfCode, &result.StfFullName, &result.DPcode); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}
		e, err := json.Marshal(result)
		if err != nil {
			fmt.Fprintf(w, "Error %s", err)
			return
		}
		//fmt.Fprintf(w,"%s",string(e))
		fmt.Fprintf(w, "%s,", (string(e)))

	}

	// If the database is being written to ensure to check for Close
	// errors that may be returned from the driver. The query may
	// encounter an auto-commit error and be forced to rollback changes.
	rerr := rows.Close()
	if rerr != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	// Rows.Err will report the last error encountered by Rows.Scan.
	if err := rows.Err(); err != nil {
		fmt.Fprintf(w, "Error %s", err)
	}
	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

//ดึงข้อมูล 1 Record
func ExampleDB_QueryRowContext(w http.ResponseWriter, r *http.Request) {

	var strSTFFullname string
	err := db.QueryRowContext(ctx, "SELECT StfFullname FROM Staff WHERE StfCode=@p1", "1233").Scan(&strSTFFullname)

	switch {

	case err == sql.ErrNoRows:

		fmt.Fprintf(w, "[]")

	case err != nil:

		fmt.Fprintf(w, "query error: %v\n", err)

	default:

		fmt.Fprintf(w, "username is %s, account created on %s\n", strSTFFullname, strSTFFullname)

	}

}

func ExampleDB_ExecContext(w http.ResponseWriter, r *http.Request) {

	strDataBaseName = "AnalysisDB"
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	result, err := db.ExecContext(ctx, "UPDATE Department SET DPShort = 'D' WHERE DPcode=@p1", "XXX")

	//ถ้าไม่เท่ากับ nil แสดงว่ามี Error เกิดขึ้น
	if err != nil {
		fmt.Fprintf(w, "Eror %s", err)
	}

	rows, err := result.RowsAffected()

	if err != nil {
		fmt.Fprintf(w, "Eror %s", err)
	}

	if rows == 0 {
		fmt.Fprintf(w, "Not found data for update")
	}

	if rows != 0 {
		fmt.Fprintf(w, "expected to affect 1 row, affected %d", rows)
	}
}

func ExampleDB_Query_multipleResultSets(w http.ResponseWriter, r *http.Request) {

	var strStfcode string
	r.ParseForm()
	strStfcode = r.FormValue("strStfcode")
	strDataBaseName = "AnalysisDB"
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	q := `select Stfcode into Tmp_Staff1 from Staff where Stfcode >= @p1; -- Populate temp table.
-- First result set.
select	Tmp_Staff1.StfCode, Staff.StfFullname
from Tmp_Staff1 inner join Staff on Tmp_Staff1.StfCode = Staff.StfCode;

-- Second result set.
select	Users.Stfcode, Users.User_Level
from Tmp_Staff1  inner join Users on Tmp_Staff1.Stfcode = Users.Stfcode;
drop table Tmp_Staff1;`

	rows, err := db.Query(q, strStfcode)

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	defer rows.Close()

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var (
			stfcode string
			name    string
		)

		if err := rows.Scan(&stfcode, &name); err != nil {

			fmt.Fprintf(w, "Error %s", err)

		}

		fmt.Fprintf(w, "stfcode %s name is %s\n", stfcode, name)

	}

	if !rows.NextResultSet() {

		log.Fatalf("expected more result sets: %v", rows.Err())

	}

	CountRecord = 0
	for rows.Next() {
		CountRecord = 1
		var (
			stfcode    string
			User_Level string
		)

		if err := rows.Scan(&stfcode, &User_Level); err != nil {

			fmt.Fprintf(w, "Error %s", err)

		}

		fmt.Fprintf(w, "Stfcode %s has Level %s\n", stfcode, User_Level)

	}

	if err := rows.Err(); err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	//แสดงว่าไม่พบ Recode
	if CountRecord == 0 {
		fmt.Fprintf(w, "[]")
	}
}

func ExampleDB_PingContext(w http.ResponseWriter, r *http.Request) {

	// Ping and PingContext may be used to determine if communication with

	// the database server is still possible.

	//

	// When used in a command line application Ping may be used to establish

	// that further queries are possible; that the provided DSN is valid.

	//

	// When used in long running service Ping may be part of the health

	// checking system.
	strDataBaseName = "TestDBName"
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	ctx, cancel := context.WithTimeout(ctx, 1*time.Second)

	defer cancel()

	status := "up"

	if err := db.PingContext(ctx); err != nil {

		status = "down"

	}
	fmt.Fprintf(w, "Error %s", status)

}

func ExampleDB_Prepare(w http.ResponseWriter, r *http.Request) {
	strDataBaseName = "AnalysisDB"
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	projects := []struct {
		mascot  string
		release int
	}{
		{"tux", 1991},
		{"duke", 1996},
		{"gopher", 2009},
		{"moby dock", 2013},
	}

	stmt, err := db.Prepare("INSERT INTO projects(id, mascot, release, category) VALUES( @p1, @p2, @p3, @p4 )")

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	defer stmt.Close() // Prepared statements take up server resources and should be closed after use.

	for id, project := range projects {

		if _, err := stmt.Exec(id+1, project.mascot, project.release, "open source"); err != nil {
			fmt.Fprintf(w, "Error %s", err)
		}

	}

}

func ExampleTx_Prepare(w http.ResponseWriter, r *http.Request) {
	strDataBaseName = "AnalysisDB"
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	projects := []struct {
		mascot  string
		release int
	}{
		{"tux", 1991},
		{"duke", 1996},
		{"gopher", 2009},
		{"moby dock", 2013},
	}

	tx, err := db.Begin()

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}
	//ทำการทดสอบ error
	defer tx.Rollback() // The rollback will be ignored if the tx has been committed later in the function.
	stmt, err := tx.Prepare("Delete From projects Where id=@p1 and id<>4 ; INSERT INTO projects(id, mascot, release, category) VALUES(  @p2, @p3, @p4, @p5 )")

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	defer stmt.Close() // Prepared statements take up server resources and should be closed after use.

	for id, project := range projects {

		if _, err := stmt.Exec(id+1, id+1, project.mascot, project.release, "open source"); err != nil {

			fmt.Fprintf(w, "Error %s", err)

		}

	}

	if err := tx.Commit(); err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

}

func ExampleStmt(w http.ResponseWriter, r *http.Request) {

	// In normal use, create one Stmt when your process starts.
	strDataBaseName = "AnalysisDB"
	ConnectServer(strDataBaseName)
	defer db.Close()
	ctx := context.Background()
	err := db.PingContext(ctx)
	if err != nil {
		fmt.Fprintf(w, "Error pinging database:"+err.Error())
	}

	stmt, err := db.PrepareContext(ctx, "SELECT StfFullname FROM Staff WHERE Stfcode = @p1")

	if err != nil {

		fmt.Fprintf(w, "Error %s", err)

	}

	defer stmt.Close()

	// Then reuse it each time you need to issue the query.

	stfcode := "1233"

	var StfFullname string

	err = stmt.QueryRowContext(ctx, stfcode).Scan(&StfFullname)

	switch {

	case err == sql.ErrNoRows:

		fmt.Fprintf(w, "[]")

	case err != nil:

		fmt.Fprintf(w, "Error %s", err)

	default:

		fmt.Fprintf(w, "username is %s\n", StfFullname)

	}

}

/*


func ExampleConn_ExecContext(w http.ResponseWriter, r *http.Request) {

	conn, err := db.Conn(ctx)

	if err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	defer conn.Close() // Return the connection to the pool.

	id := 41

	result, err := conn.ExecContext(ctx, `UPDATE balances SET balance = balance + 10 WHERE user_id = ?;`, id)

	if err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	rows, err := result.RowsAffected()

	if err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	if rows != 1 {

		log.Fatalf("expected single row affected, got %d rows affected", rows)

	}

}

func ExampleTx_ExecContext(w http.ResponseWriter, r *http.Request) {

	tx, err := db.BeginTx(ctx, &sql.TxOptions{Isolation: sql.LevelSerializable})

	if err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	id := 37

	_, execErr := tx.ExecContext(ctx, "UPDATE users SET status = ? WHERE id = ?", "paid", id)

	if execErr != nil {

		if rollbackErr := tx.Rollback(); rollbackErr != nil {

			log.Fatalf("update failed: %v, unable to rollback: %v\n", execErr, rollbackErr)

		}

		log.Fatalf("update failed: %v", execErr)

	}

	if err := tx.Commit(); err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

}

func ExampleTx_Rollback(w http.ResponseWriter, r *http.Request) {

	tx, err := db.BeginTx(ctx, &sql.TxOptions{Isolation: sql.LevelSerializable})

	if err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	id := 53

	_, err = tx.ExecContext(ctx, "UPDATE drivers SET status = ? WHERE id = ?;", "assigned", id)

	if err != nil {

		if rollbackErr := tx.Rollback(); rollbackErr != nil {

			log.Fatalf("update drivers: unable to rollback: %v", rollbackErr)

		}

		fmt.Fprintf(w,"Error %s",err)

	}

	_, err = tx.ExecContext(ctx, "UPDATE pickups SET driver_id = $1;", id)

	if err != nil {

		if rollbackErr := tx.Rollback(); rollbackErr != nil {

			log.Fatalf("update failed: %v, unable to back: %v", err, rollbackErr)

		}

		fmt.Fprintf(w,"Error %s",err)

	}

	if err := tx.Commit(); err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

}

func ExampleStmt(w http.ResponseWriter, r *http.Request) {

	// In normal use, create one Stmt when your process starts.

	stmt, err := db.PrepareContext(ctx, "SELECT username FROM users WHERE id = ?")

	if err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	defer stmt.Close()

	// Then reuse it each time you need to issue the query.

	id := 43

	var username string

	err = stmt.QueryRowContext(ctx, id).Scan(&username)

	switch {

	case err == sql.ErrNoRows:

		log.Fatalf("no user with id %d", id)

	case err != nil:

		fmt.Fprintf(w,"Error %s",err)

	default:

		fmt.Fprintf(w,"username is %s\n", username)

	}

}


func ExampleStmt_QueryRowContext(w http.ResponseWriter, r *http.Request) {

	// In normal use, create one Stmt when your process starts.

	stmt, err := db.PrepareContext(ctx, "SELECT username FROM users WHERE id = ?")

	if err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	defer stmt.Close()

	// Then reuse it each time you need to issue the query.

	id := 43

	var username string

	err = stmt.QueryRowContext(ctx, id).Scan(&username)

	switch {

	case err == sql.ErrNoRows:

		log.Fatalf("no user with id %d", id)

	case err != nil:

		fmt.Fprintf(w,"Error %s",err)

	default:

		fmt.Fprintf(w,"username is %s\n", username)

	}

}

func ExampleRows(w http.ResponseWriter, r *http.Request) {

	age := 27

	rows, err := db.QueryContext(ctx, "SELECT name FROM users WHERE age=?", age)

	if err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	defer rows.Close()

	names := make([]string, 0)

	for rows.Next() {

		var name string

		if err := rows.Scan(&name); err != nil {

			fmt.Fprintf(w,"Error %s",err)

		}

		names = append(names, name)

	}

	// Check for errors from iterating over rows.

	if err := rows.Err(); err != nil {

		fmt.Fprintf(w,"Error %s",err)

	}

	fmt.Fprintf(w,"%s are %d years old", strings.Join(names, ", "), age)

}
*/

$(function () {
    //NewStudents(20);  // default จำนวนนักเรียน 20 ถ้าจะเพิ่มห้องใหม่            

    $('#DialogChangeNoOfStudent').dialog({ autoOpen: false, draggable: false, resizable: false, modal: true, width: 200 });

    $('#btnChangeNoOfStudent').click(function () {
        $('#DialogChangeNoOfStudent').dialog('open').dialog('option', 'title', 'เพิ่ม/ลด จำนวนนักเรียน');
        $('#lblNoOfStudent').html('จาก <b>' + $('.Students').children('.StudentId').length + '</b> เป็น ');
    });
    // confirm dialog
    $('#btnConfirmChangeNoOfStudent').click(function () {
        var n = parseInt($('#txtNoOfStudent').val());
        ValidNoOfStudent(n);
        $('#txtNoOfStudent').val('');
        $('#DialogChangeNoOfStudent').dialog('close');
        if (n == "" || n == 0) {
            alert("กรอกจำนวนนักเรียนอย่างน้อย 1 คนค่ะ!"); return false;
        }
        ChangeStudent(n);
    });
    //close dialog
    $('#btnCancleChangeNoOfStudent').click(function () { $('#txtNoOfStudent').val(''); $('#DialogChangeNoOfStudent').dialog('close'); });
});

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n); // check ว่าเป็น number หรือเปล่า
}

function validStudentId(sid) {

}

function ValidNoOfStudent(n) {
    if (isNaN(n)) {
        alert("กรอกจำนวนนักเรียนเป็นตัวเลขเท่านั้นค่ะ!!!");
        return false;
    }
    if (n <= 0) {
        alert("จำนวนนักเรียนต้องมากกว่า 0 ค่ะ!!!");
        return false;
    }
}

function NewStudents(n) {
    $('.Students').html('');
    for (var i = 1; i <= n; i++) {        
        var s = GetHtmlStudent(i, '');
        $('.Students').append(s);
    }
    SetNoOfStudent();
}

function ChangeStudent(n) {
    var jsonStudent = GetJsonStudents();
    if (jsonStudent == null) {
        console.log('case ปรับจำนวนเด็กในห้อง ยังไม่ได้กรอกข้อมูล');
        NewStudents(n);
    }
    else if (jsonStudent != null) {
        console.log('case ปรับจำนวนเด็กในห้อง กรอกข้อมูลแล้ว');
        UpdateStudent(n, jsonStudent);
    }
}

function UpdateStudent(n, data) {    
    var l = GetJsonLength(data); // จำนวนของ json
    //n = GetN(n);
    var j = 0;
    $('.Students').html('');
    for (var i = 1; i <= n; i++) {
        var sid = '';
        if (j < l && i == data.students[j].number) {
            sid = data.students[j].studentid;
            j++;
        }        
        var s = GetHtmlStudent(i, sid);
        $('.Students').append(s);
    }    
    SetNoOfStudent();
}

function GetJsonStudents() {
    var students = '';
    $('.Students').children('.StudentId').each(function () {
        var number = $(this).children().html();
        var sid = $(this).children().next().val(); // เอาค่า studentid จาก textbox                    
        if (sid != "") {
            // กรอกรหัสนักเรียนเป็นตัวหนังสือก็ได้
            //if (!isNumber(sid)) {
            //    alert("กรอกรหัสนักเรียนเป็นตัวเลขเท่านั้นค่ะ!");
            //    return false;
            //}
            students += '{"number":"' + number + '","studentid":"' + sid + '"},';
        }
    });
    students = '{"students":[' + students.substring(0, students.length - 1) + ']}'; console.log(students);
    students = JSON.parse(students);
    var r = (GetJsonLength(students) > 0 ? students : null);
    return r;
}

function GetJsonLength(data) { // get จำนวนของ obj json            
    var count = 0;
    for (key in data.students) {
        if (data.students.hasOwnProperty(key)) {
            count++;
        }
    }
    return count;
}

function GetHtmlStudent(i, sid) {
    //return '<div class="StudentId"><div class="numberCircle">' + i + '</div><input type="text" id="txtnumber' + i + '" class="txtStudentId" style="width:80px;" value="' + sid + '" maxlength="5" onkeypress="return event.charCode >= 48 && event.charCode <= 57" onkeyup="SetNoOfStudent();" /></div>';
    return '<div class="StudentId"><div class="numberCircle">' + i + '</div><input type="text" id="txtnumber' + i + '" class="txtStudentId" maxlength="50"  value="' + sid + '"  onkeyup="SetNoOfStudent();" /></div>';
}

function GetNoOfStudentFromtxtNumber() {
    var count = 0;
    $('.Students').children('.StudentId').each(function () {
        if ($(this).children().next().val() != "") {
            count++;
        }
    });
    return count;
}

function SetNoOfStudent() {
    $('.NoOfStudent').text('จำนวนนักเรียน ' + GetNoOfStudentFromtxtNumber() + ' คน');
}

function CheckDataJson(oldData,newData) {
    var noOfOldData = GetJsonLength(oldData);
    var noOfNewData = GetJsonLength(newData);
    var students;
    for (var i = 0; i < noOfOldData; i++) {
        var number = oldData.students[i].number;
        var studentid = oldData.students[i].studentid;
        var s = CompareDataJson(number, studentid, newData, noOfNewData);
        if (s != null) {
            students += s;
        }
    }
    return '{"students":[' + students.substring(0, students.length - 1) + ']}'; 
}

function CompareDataJson(number, studentid, data2, n) {
    for (var j = 0; j < n; j++) {
        if (number == data2.students[j].number) {
            if (studentid == data2.students[j].studentid) {
            }
            break;
            return null;
        }
    }
    return '{"number":"' + number + '","studentid":"' + studentid + '"},';
}

var isRoomManagementCallback = false;

$('.txtStudentId').live('keyup', function (e) {
    var code = e.keyCode || e.which;
    if (code == '9') { // tab key
        return false;
    }
    var students = GetJsonStudents();   
    var delayID = null; 
    if (delayID == null) {
        if (isRoomManagementCallback == false) {
            delayID = TimeOutCheckDuplicateStudent(students);
        } else {
            $(this).removeClass('duplicateNumber');
        }
    }
    //else if (delayID != null) {
    //    clearTimeout(delayID);
    //    delayID = (isCallback == false) ? TimeOutCheckDuplicateStudent(students) : setTimeout(function () { $(this).removeClass('duplicateNumber'); });
    //}
});

function TimeOutCheckDuplicateStudent(s) {    
    return setTimeout(function () {
        CheckDuplicateStudent(s);
    }, 1200);  
}
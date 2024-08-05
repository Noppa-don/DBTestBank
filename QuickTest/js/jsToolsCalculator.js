//Calculator
var keepNum = '';
function calculator(number) {
    var txtNum = document.getElementById('SumNumber').innerHTML;
    if (IsNumber(number)) {
        var s = keepNum.indexOf('.');
        if (txtNum == '0') {
            if (number == '.') {
                if (s == -1) {
                    document.getElementById('SumNumber').innerHTML += number;
                }
            } else {
                NoOfNumber++;
                document.getElementById('SumNumber').innerHTML = number;
            }
        } else {
            if (number == '.') {
                if (s == -1) {
                    document.getElementById('SumNumber').innerHTML += number;
                }
            } else {
                if (IsNotOverBaseCharacter(txtNum)) {
                    document.getElementById('SumNumber').innerHTML += number;
                }
            }
        }
        keepNum += number;
    } else {
        keepNum = '';
        txtNum = StrBeforeEval(txtNum);
        if (number == '=') {
            document.getElementById('SumNumber').innerHTML = eval(txtNum);
        } else {
            document.getElementById('SumNumber').innerHTML = txtNum + number;
        }
    }

}
function IsNumber(number) {
    if (number == '/' || number == '*' || number == '-' || number == '+' || number == '=') {
        NoOfNumber = 0;
        return false;
    } else {
        return true;
    }
}
function StrBeforeEval(str) {
    var s = str.substr(str.length - 1, 1);
    if (isNaN(s)) {
        str = str.substring(0, str.length - 1);
    }
    return str;
}
//กรอกตัวเลขแต่ละชุดได้ไม่เกิน 6 หลัก
var NoOfNumber = 0;
function IsNotOverBaseCharacter(Number) {
    //var ArrOperator = ['/', '*', '-', '+']; var IsHaveOperater;
    //for (var i = 0; i < ArrOperator.length - 1; i++) {
    //    if (Number.indexOf(ArrOperator[i].toString) != -1) {
    //        IsHaveOperater = true;
    //        break;
    //    }
    //    else {
    //        IsHaveOperater = false;
    //    }        
    //}
    //var Operators = '/*-+';
    //var IsHaveOperater = (Number.indexOf(Operators) != -1) ? true : false;
    //console.log(Number.indexOf(Operators));
    //var ArrNumber = Number.split(ArrOperator[0].toString);//.split(ArrOperator[1].toString).split(ArrOperator[2].toString).split(ArrOperator[3].toString);
            //alert(ArrNumber[ArrNumber.length - 1]);

    //console.log(NoOfNumber);
    //if (IsHaveOperater) {
//        var ArrNumber = Number.split(ArrOperator[0].toString).split(ArrOperator[1].toString).split(ArrOperator[2].toString).split(ArrOperator[3].toString);
        //        alert(ArrNumber[ArrNumber.length - 1]);
    //    NoOfNumber = 1;
    //}
    //else {        
        
    //}
    //console.log(NoOfNumber);
    if (NoOfNumber >= 6) {
        return false;
    }
    else {
        NoOfNumber++;
        return true;
    }
}

function ClearValue() {
    NoOfNumber = 0;
    document.getElementById("SumNumber").innerHTML = "0";
}
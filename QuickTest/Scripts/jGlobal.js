function global() {
}

global.convertJsonDateToJsDate = function (d) {
    var substringedDate = d.substring(6); //substringedDate= 1291548407008)/
    var parsedIntDate = parseInt(substringedDate); //parsedIntDate= 1291548407008
    var date = new Date(parsedIntDate);  // parsedIntDate passed to date constructor
    return date;
}

global.dateToString = function (date) {
    return date.getDate() + '/' + (date.getMonth() + 1) + '/' + (date.getFullYear() + 543);
}

global.timeToString = function (date) {
    return date.getHours() + ":" + date.getMinutes();
}

global.dateNowString = function () {
    var d = new Date()
    return d.getDate() + '/' + (d.getMonth()+1) + '/' + (d.getFullYear() + 543);
}

global.setRealDateUI = function (date) {
    // ค่าเก่าไม่เปลี่ยน + 1 เดือนเพราะ เดือนที่เป็นเลขจะน้อยกว่าเดือนจริง 1 เพราะมันมองเป็น array
    var newDate = new Date(date);
    //newDate.setMonth(date.getMonth() + 1);
    return newDate
}

global.datepickerOption = {
    dateFormat: 'dd/mm/yy',
    isBuddhist: true,
    dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
    dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
    monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
    monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
}

global.removejscssfile = function (filename, filetype) {
    var targetelement = (filetype == "js") ? "script" : (filetype == "css") ? "link" : "none"
    var targetattr = (filetype == "js") ? "src" : (filetype == "css") ? "href" : "none"
    var allsuspects = document.getElementsByTagName(targetelement)
    for (var i = allsuspects.length; i >= 0; i--) {
        if (allsuspects[i] && allsuspects[i].getAttribute(targetattr) != null && allsuspects[i].getAttribute(targetattr).indexOf(filename) != -1)
            allsuspects[i].parentNode.removeChild(allsuspects[i])
    }
}

global.debug = function (msg) {
    console.debug(msg);
}
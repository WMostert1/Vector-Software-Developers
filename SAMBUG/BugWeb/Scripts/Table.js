$(document).ready(function () {
    setFromDate();
    setToDate();
});

function generate()
{
/*    alert("hello");*/
};


function setFromDate() {
    var date = new XDate();
    var newDate = date.addMonths(-6, true);
    $("#timeFrom").val(newDate.toString("yyyy-MM-dd"));

};

function setToDate() {
    var date = new XDate();
    $("#timeTo").val(date.toString("yyyy-MM-dd"));
}








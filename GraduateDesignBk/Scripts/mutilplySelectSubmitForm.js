//全选 
function CheckDelete() {
    var allCheck = document.getElementById("Totalselect");
    var check = document.getElementsByName("delId");
    if (allCheck.checked == false) {
        allCheck.checked = false;
        for (var i = 0; i < check.length; i++) {
            check[i].checked = false;
        }
        return;
    }
    else {
        for (var i = 0; i < check.length; i++) {
            check[i].checked = true;
        }
    }
}
//得到所有checkbox的name=delId的值得字符串
function getIds() {
    var ids = "";
    var check = document.getElementsByName("delId");
    for (var i = 0; i < check.length; i++) {
        if (check[i].checked == true) {
            ids += check[i].value + "|";
        }
    }
    return ids.substring(0, ids.length - 1);
}
//使用超链接提交表单，得到所有被选中的行的元素Id提交表单
function Submit(name) {
    var ids = getIds();
    if (ids != "") {
        $("#" + name).val(ids);
        $('#form_' + name).submit();
    } else {
        alert("你未选中任何选项！");
    }
}
//跳页时将每页显示数量提交上去
function getPageSize(type) {
    if (type == 1) {
        $("#CurIndex").val($("#CurIndex").val() - 1);
    } else if (type == 2) {
        $("#CurIndex").val($("#CurIndex").val() - (-1));
    }
    $("#pagesize").val($("#PageSize").val());
    $("#form_Search").submit();
}
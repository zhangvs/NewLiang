// JavaScript Document

//����Ԣ���������
var g = new Array();
g[0] = new Array("1314520", "һ��һ���Ұ���");
g[1] = new Array("259695", "���Ҿ��˽���");
g[2] = new Array("74839", "��ʵ������");
g[3] = new Array("829575", "���������Ҹ�");
g[4] = new Array("20863", "���㵽����");
g[5] = new Array("594320", "�Ҿ����밮��");
g[6] = new Array("51020", "����Ȼ����");
g[7] = new Array("520", "�Ұ���");
g[8] = new Array("1314920", "һ��һ���Ͱ���");
g[9] = new Array("584520", "�ҷ����Ұ���");
g[10] = new Array("04551", "������Ψһ");
g[11] = new Array("77543", "�²�����˭");
g[12] = new Array("2010000", "����һ����");
g[13] = new Array("3344520", "���������Ұ���");

function GetMobileArray(strInput) {
    var reg = new RegExp(/1[3-9]\d{9}/ig);
    var r = strInput.match(reg); // ����ƥ�������ַ�����
    return (r);
}

/*
function GetMobileArray(var strInput)
{
var reg = new   RegExp(/^1[3|5]\d{9}$/);
var r = strInput.match(reg); // ����ƥ�������ַ�����
return(r); 
}
*/

function SetMobileStyle(style) {
    if (style == 2) {
        SetMobileStyle2();
    }
    else {
        SetMobileStyle1();
    }
}

function SetMobileStyle_(style,d) {
    if (style == 2) {
        return SetMobileStyle2_(d);
    }
    else {
        return SetMobileStyle1_(d);
    }
}

function SetMobileStyle1() {
    $(".TBmobileData").each(function () {
        $(this).html(SetMobileStyle1_($(this).html()));
    });
}

function SetMobileStyle1_(myData) {
    var re = GetMobileArray(myData);
    if (re != null)//�ֻ��������鲻Ϊ��
    {
        //�������滻
        for (var i = 0; i < re.length; i++) {
            //myData=myData.replace(re[i],'<font color=red>'+re[i]+'</font>');
            var oldvalue = re[i];
            var newvalue = re[i];

            //����2 , ���������ظ�3������(��3��) , ��Ϊ��ɫ
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)(\1{3,})/ig, "<span style='color:#FF6600;'>$1$2</span>");

            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(01234567|12345678|23456789|98765432|87654321|76543210|0123456|1234567|2345678|3456789|9876543|8765432|7654321|6543210|012345|123456|234567|345678|456789|987654|876543|765432|654321|543210|01234|12345|23456|34567|45678|56789|98765|87654|76543|65432|54321|43210|0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210)/ig, "<font color=red>$1</font>")
            //����1 , 4�������ϵ�˳��(��4��)

            //����2 , ���������ظ�3������(��3��) , ��Ϊ��ɫ
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)(\1{2,})/ig, "<span style='color:#FF6600;'>$1$2</span>");

            //����3 , ���-�ظ� ABABAB
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)([^\1])(\1)(\2)(\1)(\2)/ig, "<font color=red>$1</font><font color=blue>$2</font><font color=red>$3</font><font color=blue>$4</font><font color=red>$5</font><font color=blue>$6</font>");

            //����4 , ���-�ظ� ABABA
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)([^\1])(\1)(\2)(\1)/ig, "<font color=red>$1</font><font color=blue>$2</font><font color=red>$3</font><font color=blue>$4</font><font color=red>$5</font>");

            //����5 , ���-�ظ� ABAB
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)([^\1])(\1)(\2)/ig, "<font color=red>$1</font><font color=blue>$2</font><font color=red>$3</font><font color=blue>$4</font>");

            //����6 , ���-�ظ� AABBCC|AABBAA
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)(\1)([^\1])(\3)([^\3])(\5)/ig, "<font color=red>$1$2</font><font color=blue>$3$4</font><font color=green>$5$6</font>");

            //����7 , ���-�ظ� AABB
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)(\1)([^\1])(\3)/ig, "<font color=red>$1$2</font><font color=blue>$3$4</font>");

            //����8 , ����Ԣ���������
            /*
            for (var k=0; k < g.length; k++)
            {
            newvalue=newvalue.replace(g[k][0],"<font color=red title="+g[k][1]+">"+g[k][0]+"</font>"+"<font color=gray  size='1'>("+g[k][1]+")</font>");
            }
            */

            myData = myData.replace(oldvalue, newvalue);
        }
    }
    return myData;
}

function SetMobileStyle2() {
    $(".TBmobileData").each(function () {
            $(this).html(SetMobileStyle2_($(this).html()));
    });
}

function SetMobileStyle2_(myData) {
    var re = GetMobileArray(myData);
    if (re != null)
    {
        for (var i = 0; i < re.length; i++) {
            var oldvalue = re[i];
            var newvalue = re[i];
            newvalue = newvalue.replace(/^(\d{3})(\d{4})(\d{4})$/ig, "$1<font color=#FF8C69>$2</font><font color=#00BFFF>$3</font>");
            myData = myData.replace(oldvalue, newvalue);
        }
    }
    return myData;
}
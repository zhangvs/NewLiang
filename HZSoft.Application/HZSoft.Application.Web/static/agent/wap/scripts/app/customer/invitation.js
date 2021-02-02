$(function () {
    var clipboard = new ClipboardJS('.copy');
    clipboard.on('success', function (e) {
        e.clearSelection();
        $.toast('复制成功');
    });

    clipboard.on('error', function (e) {
        $.toast('复制失败，请手动长按复制');
    });

    $('#share').click(function () {
        $.toast('请点击右上角微信分享');
    });
});

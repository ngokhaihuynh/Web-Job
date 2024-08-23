$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddSave', function (e) {
        e.preventDefault();
        var id = $(this).data('id');

        $.ajax({
            url: '/savejob/addsavejob',
            type: 'POST',
            data: { id: id },
            success: function (rs) {
                /*console.log(rs);*/
                if (rs.success) {
                    $('#checkout_items').html(rs.Count);
                    alert(rs.msg);
                }
            }
        });

    });

    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có muốn xóa công việc đã lưu?');
        if (conf == true) {
            $.ajax({
                url: '/savejob/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    /*console.log(rs);*/
                    if (rs.success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove();
                    }
                }
            });
        }
       

    });
});

function ShowCount() {
    $.ajax({
        url: '/savejob/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.Count);
        }
    });
}
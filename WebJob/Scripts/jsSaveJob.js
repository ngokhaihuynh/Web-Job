////$(document).ready(function () {
////    ShowCount();
////    $('body').on('click', '.btnAddSave', function (e) {
////        e.preventDefault();
////        var id = $(this).data('id');

////        $.ajax({
////            url: '/savejob/addsavejob',
////            type: 'POST',
////            data: { id: id },
////            success: function (rs) {
////                /*console.log(rs);*/
////                if (rs.success) {
////                    $('#checkout_items').html(rs.Count);
////                    alert(rs.msg);
////                }
////            }
////        });

////    });

////    $('body').on('click', '.btnDelete', function (e) {
////        e.preventDefault();
////        var id = $(this).data('id');
////        var conf = confirm('Bạn có muốn xóa công việc đã lưu?');
////        if (conf == true) {
////            $.ajax({
////                url: '/savejob/Delete',
////                type: 'POST',
////                data: { id: id },
////                success: function (rs) {
////                    /*console.log(rs);*/
////                    if (rs.success) {
////                        $('#checkout_items').html(rs.Count);
////                        $('#trow_' + id).remove();
////                    }
////                }
////            });
////        }
       

////    });
////});

////function ShowCount() {
////    $.ajax({
////        url: '/savejob/ShowCount',
////        type: 'GET',
////        success: function (rs) {
////            $('#checkout_items').html(rs.Count);
////        }
////    });
////}

var SaveJobModule = {
    init: function () {
        this.showCount();
        this.LoadSaveJob();
       
        this.setupEventHandlers();
    },

    showCount: function () {
        $.ajax({
            url: '/savejob/ShowCount',
            type: 'GET',
            success: function (rs) {
                $('#checkout_items').html(rs.Count);
            }
        });
    },
    LoadSaveJob: function () {
        $.ajax({
            url: '/savejob/index',
            type: 'GET',
            success: function (rs) {
                $('#load_data').html(rs.Count);
            }
        });
    },
    DeleteAllJob: function () {
       // Thêm dòng này để kiểm tra
        $.ajax({
            url: '/SaveJob/DeleteAllJob',
            type: 'POST',
            success: function (rs) {
                console.log(rs); // Kiểm tra phản hồi từ server
                if (rs.success) {
                    CartModule.LoadSaveJob(); // Làm mới giỏ hàng
                    CartModule.showCount(); // Cập nhật số lượng
                    alert("Xóa toàn bộ giỏ hàng thành công.");
                } else {
                    alert("Không thể xóa tất cả sản phẩm trong giỏ hàng");
                }
            },
            error: function () {
                alert("Lỗi kết nối với server");
            }
        });
    },

    setupEventHandlers: function () {
        $('body').on('click', '.btnAddSave', function (e) {
            e.preventDefault();
            var id = $(this).data('id');

            $.ajax({
                url: '/savejob/addsavejob',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
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
                        if (rs.success) {
                            $('#checkout_items').html(rs.Count);
                            $('#trow_' + id).remove();
                        } else {
                            alert('Không thể xóa công việc. Vui lòng thử lại sau.');
                        }
                    },
                    error: function () {
                        alert('Có lỗi xảy ra trong quá trình xóa.');
                    }
                });
            }
        });
    }
};

$(document).ready(function () {
    SaveJobModule.init();
});

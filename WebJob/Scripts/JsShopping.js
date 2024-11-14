////$(document).ready(function () {
////    ShowCount();
////    $('body').on('click', '.btnAddToCart', function (e) {
////        e.preventDefault();
////        var id = $(this).data('id');
////        var quantity = 1;
        
////        $.ajax({
////            url: '/shoppingcart/addtocart',
////            type: 'POST',
////            data: { id: id, quantity: quantity },
////            success: function (rs) {
////                /*console.log(rs);*/
////                if (rs.success) {
////                    $('#checkout_cart').html(rs.Count);
////                    ShowCount();
////                    alert(rs.msg);
////                }
////            }
////        });

////    });

////    $('body').off('click', '.btnDelete').on('click', '.btnDelete', function (e) {
////        e.preventDefault();
////        var id = $(this).data('id');
////        var conf = confirm('Bạn có muốn xóa công việc đã lưu?');
////        if (conf == true) {
////            $.ajax({
////                url: '/shoppingcart/Delete',
////                type: 'POST',
////                data: { id: id },
////                success: function (rs) {
////                    if (rs.success) {
////                        $('#checkout_cart').html(rs.Count);
////                        $('#trow_' + id).remove();
////                        location.reload();
////                    }
////                }
////            });
////        }
////    });

////});

////function ShowCount() {
////    $.ajax({
////        url: '/shoppingcart/ShowCount',
////        type: 'GET',
////        success: function (rs) {
////           // console.log(rs);
////            $('#checkout_cart').html(rs.Count);
////        }
////    });
////}

var CartModule = {
    init: function () {
        this.showCount();
        this.LoadCart();
        this.setupEventHandlers();
    },


    showCount: function () {
        $.ajax({
            url: '/shoppingcart/ShowCount',
            type: 'GET',
            success: function (rs) {
                $('#checkout_cart').html(rs.Count);
            }
        });
    },

    LoadCart: function () {
        $.ajax({
            url: '/shoppingcart/Partial_Iteam_Cart_Page',
            type: 'GET',
            success: function (rs) {
                $('#load_data').html(rs); // Load lại nội dung giỏ hàng
            }
        });
    },
    //DeleteAll: function () {
    //    $.ajax({
    //        url: '/shoppingcart/DeleteAll',
    //        type: 'POST',
    //        success: function (rs) {
    //            if (rs.success) {
    //                CartModule.LoadCart(); // Load lại giỏ hàng
    //                alert(rs.message);
    //            } else {
    //                alert("Không thể xóa tất cả sản phẩm trong giỏ hàng");
    //            }
    //        },
    //        error: function (jqXHR, textStatus, errorThrown) {
    //            console.error("Error:", textStatus, errorThrown);
    //            alert("Không thể kết nối với server");
    //        }
    //    });
    //},




    setupEventHandlers: function () {
        $('body').on('click', '.btnAddToCart', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var quantity = 1;

            $.ajax({
                url: '/shoppingcart/addtocart',
                type: 'POST',
                data: { id: id, quantity: quantity },
                success: function (rs) {
                    if (rs.success) {
                        $('#checkout_cart').html(rs.Count);
                        CartModule.showCount();
                        alert(rs.msg);
                    }
                }
            });
        });

        $('body').on('click', '.btnDeleteAll', function (e) {
            e.preventDefault();
            console.log('Button Xóa tất cả clicked'); // Kiểm tra xem sự kiện có chạy không
            var conf = confirm('Bạn có muốn xóa hết sản phẩm khỏi giỏ hàng?');
            if (conf) {
                CartModule.DeleteAll();
            }
        });


        $('body').off('click', '.btnDeleteProduct').on('click', '.btnDeleteProduct', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var conf = confirm('Bạn có muốn xóa sản phẩm khỏi giỏ hàng?');
            if (conf == true) {
                $.ajax({
                    url: '/shoppingcart/Delete',
                    type: 'POST',
                    data: { id: id },
                    success: function (rs) {
                        if (rs.success) {
                            $('#checkout_cart').html(rs.Count);
                            $('#trow_' + id).remove();
                            location.reload();
                        }
                    }
                });
            }
        });
       
    }

};

$(document).ready(function () {
    CartModule.init();
});
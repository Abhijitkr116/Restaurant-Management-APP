

     $(document).ready(function () {
         alert("Ready");
         $('#BookingDate, #BookingTime, #Duration').on('change', function () {
             alert("Hello");
             var bookingDate = $('#BookingDate').val();
             var bookingTime = $('#BookingTime').val();
             var bookingTimeDuration = $('#Duration').val();

             if (bookingDate && bookingTime) {
                 debugger;
                 $.ajax({
                     url: '@Url.Action("GetAvailableTables", "Restaurant")',
                     type: 'GET',
                     data: { bookingDate: bookingDate, bookingTime: bookingTime, duration: bookingTimeDuration },
                     success: function (data) {
                         var tableSelect = $('#tableId');
                         tableSelect.empty();
                         data.forEach(function (table) {
                             var option = $('<option></option>').attr('value', table.TableId)
                                 .text('Table ' + table.TableNumber + ' (Seats: ' + table.Capacity + ')');
                             tableSelect.append(option);
                         });
                     },
                     error: function (error) {
                         console.error('Error fetching tables:', error);
                     }
                 });
             }
         });
     });
     
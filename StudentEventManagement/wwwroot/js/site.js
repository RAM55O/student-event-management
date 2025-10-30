$(document).ready(function(){
    // Function to toggle event details with animation
    window.toggleDetails = function(eventId) {
        var details = $('#details-' + eventId);
        var btnText = $('#btn-' + eventId);

        details.slideToggle('fast', function() {
            if (details.is(':visible')) {
                btnText.text('Less Details');
            } else {
                btnText.text('More Details');
            }
        });
    }
});

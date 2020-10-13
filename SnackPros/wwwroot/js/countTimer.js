setInterval(countdown, 60000);



//function countdown() {
//    var daysLeft = Math.floor(timeLeft / (1000 * 60 * 60 * 24));
//    var hoursLeft = Math.floor((timeLeft / (1000 * 60 * 60)) % 24);
//    var minutesLeft = Math.floor((timeLeft / 1000) / 60 % 60);

//    const timeLeftText = `${daysLeft} days ${hoursLeft} hours ${minutesLeft} minutes.`;
//    document.getElementById("timer").innerHTML = timeLeftText;
//}
//countdown();

function countdown() {
    var timeLeft = pickUpTime - new Date();
    var daysLeft = Math.floor(timeLeft / (1000 * 60 * 60 * 24));
    var hoursLeft = Math.floor((timeLeft / (1000 * 60 * 60)) % 24);
    var minutesLeft = Math.floor((timeLeft / 1000) / 60 % 60);

    const timeLeftText = `${daysLeft} days ${hoursLeft} hours ${minutesLeft} minutes.`;
    x = document.getElementsByClassName("timer");
    for (var i = 0; i < x.length; i++) {
        x[i].innerHTML = timeLeftText;
    }
   // document.getElementsByClassName("timer")[0].innerHTML = timeLeftText;
  
};

countdown();

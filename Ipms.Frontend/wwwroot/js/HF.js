//function nextStep(step) {

//	document.querySelectorAll('.step').forEach(el => {
//		el.classList.remove('active');
//	});

//	document.getElementById("step" + step).classList.add('active');

//	updateProgress(step);

//}
document.getElementById("step1Form").addEventListener("submit", function (e) {

	e.preventDefault();   // prevent page reload

	if (this.checkValidity()) {
		nextStep(2);      // go to next step only if valid
	}

});

//document.getElementById("step1Form").addEventListener("submit", function (e) {

//	e.preventDefault();

//	if (this.checkValidity()) {
//		nextStep(2);
//	} else {
//		this.reportValidity(); // shows "Please fill out this field"
//	}

//});

function prevStep(step) {

	document.querySelectorAll('.step').forEach(el => {
		el.classList.remove('active');
	});

	document.getElementById("step" + step).classList.add('active');

	updateProgress(step);

}

function updateProgress(step) {

	let circles = document.querySelectorAll('.circle');

	circles.forEach((circle, index) => {
		circle.classList.remove('active');

		if (index < step)
			circle.classList.add('active');
	});

}

function FastButton(element, handler) {
        this.element = element;
        this.handler = handler;
        element.addEventListener('touchstart', this, false);
        element.addEventListener('click', this, false);
    };

         FastButton.prototype.handleEvent = function(event) {
            switch (event.type) {
               case 'touchstart': this.onTouchStart(event); break;
               case 'touchmove': this.onTouchMove(event); break;
               case 'touchend': this.onClick(event); break;
               case 'click': this.onClick(event); break;
            }
         };
         FastButton.prototype.onTouchStart = function(event) {
            event.stopPropagation();
            this.element.addEventListener('touchend', this, false);
            document.body.addEventListener('touchmove', this, false);
            this.startX = event.touches[0].clientX;
            this.startY = event.touches[0].clientY;
             //this.element.style.backgroundColor = "rgba(0,0,0,.7)";
            this.element.style.background = "rgba(0,0,0,.7)";
         };
         FastButton.prototype.onTouchMove = function(event) {
            if(Math.abs(event.touches[0].clientX - this.startX) > 10 || Math.abs(event.touches[0].clientY - this.startY) > 10) {
               this.reset();
            }
         };
         FastButton.prototype.onClick = function(event) {
            event.stopPropagation();
            this.reset();
            this.handler(event);
            if(event.type == 'touchend') {
               preventGhostClick(this.startX, this.startY);
            }
             //this.element.style.backgroundColor = "";
            //this.element.style.background = "";
         };
         FastButton.prototype.reset = function() {
            this.element.removeEventListener('touchend', this, false);
            document.body.removeEventListener('touchmove', this, false);
             //this.element.style.backgroundColor = "";
            this.element.style.background = "";
         };
         function preventGhostClick(x, y) {
            coordinates.push(x, y);
            window.setTimeout(gpop, 2500);
         };
         function gpop() {
            coordinates.splice(0, 2);
         };
         function gonClick(event) {
            for(var i = 0; i < coordinates.length; i += 2) {
               var x = coordinates[i];
               var y = coordinates[i + 1];
               if(Math.abs(event.clientX - x) < 25 && Math.abs(event.clientY - y) < 25) {
                  event.stopPropagation();
                  event.preventDefault();
               }
            }
         };
         document.addEventListener('click', gonClick, true);
         var coordinates = [];
         function initFastButtons() {
	//alert('aaa');
            new FastButton(document.getElementById("btn0"), goSomewhere);
         };
         function goSomewhere() {
            document.getElementById("clicklog").innerHTML = document.getElementById("clicklog").innerHTML + " Tap. ";
	//alert('bbb');
         };

         function HilightDialog(InputObj) {
             var OldBackgroundColor = $(InputObj).css('background-color');
             var OldFontColor = $(InputObj).css('color');
             $(InputObj).css('background-color', '#F6A828');
             setTimeout(function () {
                 $(InputObj).css('background-color', OldBackgroundColor);
                 $(InputObj).css('color', OldFontColor);
             }, 100);
         }

         function TriggerServerButton(e) {
             var obj = e.target;
             $(obj).trigger('click');
             //$(obj).css('border', 'solid 1px #91500A');
             //$(obj).css('text-shadow', '1px 1px #68320E');
             //$(obj).css('background', '-webkit-linear-gradient(top, #D86308 0%,#F86E02 100%)');
             //setTimeout(function () {
             //    $(obj).trigger('click');
             //}, 100);
         }
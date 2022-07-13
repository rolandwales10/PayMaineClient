app.component('selectTrknr', {
    props: {
        contextPath: String,
        messages: Array
        // TODO: receive fieldErrors from GET
    },
    data() {
        return {
            msgs: this.messages,
            trackingNbr: ""
        }
    },
    methods: {
        getData() {
            var dat = this;
            grecaptcha.execute("6LdVoW4aAAAAAAKUBs15AZsy2PSfgDReCtsqO1X5").then(function (token) {

                fetch(dat.contextPath + 'TrackingNbr/Validate?TrackingNbr=' + dat.trackingNbr)
                    .then(response => response.json())
                    .then(data => {
                        dat.msgs = data.messages;
                        if (dat.msgs.length == 0) {
                            window.location.replace(dat.contextPath + "TrackingNbr/Confirm?TrackingNbr=" + dat.trackingNbr);
                        }
                    });

            })
                .catch(function (err) {
                    console.log('Recaptcha error');
                });

        }
    },
    template:
        /*html*/
    `<main id="main" class="my-2 pb-3">
    <alert v-for="message in msgs" :status="message.status" :content="message.content"></alert>

      <p aria-hidden="true" id="required-description">
        <span class="required">*</span>Required field
      </p>
    <form v-on:submit.prevent="getData">
    <h3>Enter Tracking Number</h3>
    <div class="form-group row">
        <div class="col-md-3">
            <label for="ClientPaymentId" style="font-size: 1.5rem"> <span class="required">*</span>Tracking Number</label>
            <input id="ClientPaymentId" type="number" v-model="trackingNbr" @keyup.enter="getData" required/>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-offset-3 col-md-2">
            <br>
            <button class="btn btn-primary">
                Continue
            </button>
        </div>
    </div>
    </form>
    </main>`
})
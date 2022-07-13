app.component('Edit', {
    props: {
        contextPath: String,
        payMaineRequest: Object,
        messages: Array
        // TODO: receive fieldErrors from GET
    },
    mounted() {
        this.ti.ApplicationFee = this.dollarUS.format(this.ti.ApplicationFeeD);
    },
    data() {
        return {
            msgs: this.messages,
            pmr: this.payMaineRequest,
            ti: this.payMaineRequest.TrackingInfo,
            totalFee: "",
            dollarUS: Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
            })
        }
    },
    methods: {
        pay() {
            // POST form data with a JSON body
            const requestOptions = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "__RequestVerificationToken": document.getElementsByName('__RequestVerificationToken')[0].value
                },
                body: JSON.stringify({
                    "payMaineRequest": this.pmr
                })
            };

            fetch(this.contextPath + 'PaymentInfo/PostData', requestOptions)
                .then(response => response.json())
                .then(data => {
                    this.msgs = data.messages;
                    if (this.msgs.length == 0) {
                        this.msgs.push({ status: "info", content: "Submitting your payment information to PayMaine.  Please be patient" });
                        this.msgs.push({ status: "info", content: "You are about to be transported out of this application and taken to the payment page" });
                        window.location.replace(this.contextPath + 'Payment/RouteToBank?ClientPaymentId=' + data.payMaineRequest.TrackingInfo.CheckRecordId);
                    }
                });
        },
        calcACHFee() {
            this.ti.ApplicationTransactionFee = "$0.25";
            var totalFeeD = this.ti.BalanceDueD + 0.25;
            this.totalFee = this.dollarUS.format(totalFeeD);
        },
        calcCCFee() {
            var txnFee = this.ti.BalanceDueD * 0.03;
            this.ti.ApplicationTransactionFee = this.dollarUS.format(txnFee);
            var totalFeeD = this.ti.BalanceDueD + txnFee;
            this.totalFee = this.dollarUS.format(totalFeeD);
        }
    },
    template:
        /*html*/
        `<main id="main" class="my-2 pb-3">
        <alert v-for="message in msgs" :status="message.status" :content="message.content"></alert>

          <p aria-hidden="true" id="required-description">
            <span class="required">*</span>Required field
          </p>
        <form v-on:submit.prevent="pay">
        <div class="row g-2 mb-1">
            <div class="col-md-3">
                <label for="TrackingNbr" style="font-size: 1.5rem">Tracking Number</label>
                <input id="TrackingNbr" type="number" v-model="ti.TrackingNbr" readonly/>
            </div>
            <fieldset class="col-md-6">
                <legend><span class="required">*</span>Payment Method</legend>
                <input id="CreditCard" name="PaymentMethod" v-model="pmr.PaymentMethod" type="radio" value="CC" v-on:click="calcCCFee" required="required">
                &nbsp;
                <label for="CreditCard">Credit Card (3% will be added to the application payment amount)</label>
                <br>
                <input id="ACH" name="PaymentMethod" v-model="pmr.PaymentMethod" type="radio" value="ACH" v-on:click="calcACHFee" required="required">
                &nbsp;
                <label for="ACH">Online Check Payment ($0.25 will be added to the application payment amount)</label>
                <br>
            </fieldset>
        </div>
        <div class="row g-2 mb-1">
            <div class="col-md-3">
                    <div class="d-flex flex-column">
                <label for="BalanceDue">Balance Due</label>
                <input id="BalanceDue" v-model="ti.BalanceDue" readonly/>
                    </div>
            </div>
            <div class="col-md-3">
                    <div class="d-flex flex-column">
                <label for="TxnFee">Transaction Fee</label>
                <input id="TxnFee" v-model="ti.ApplicationTransactionFee" readonly/>
                    </div>
            </div>
            <div class="col-md-3">
                    <div class="d-flex flex-column">
                <label for="TotalFee">Total Fee</label>
                <input id="TotalFee" v-model="totalFee" readonly/>
                    </div>
            </div>
        </div>
        <br>
        <div v-if="msgs.length == 0">
            <div class="row g-2 mb-1">
                <div class="col-md-3">
                    <div class="d-flex flex-column">
                        <label for="FirstName"><span class="required">*</span>First Name</label>
                        <input id="FirstName" v-model="pmr.FirstName" required/>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="d-flex flex-column">
                        <label for="LastName"><span class="required">*</span>Last Name</label>
                        <input id="LastName" v-model="pmr.LastName" required/>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="d-flex flex-column">
                        <label for="Address"><span class="required">*</span>Street Address (no city/state)</label>
                        <input id="Address" v-model="pmr.Address" required/>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="d-flex flex-column">
                        <label for="ZipCode"><span class="required">*</span>Zip Code</label>
                        <input id="ZipCode" v-model="pmr.ZipCode" required/>
                    </div>
                </div>
            </div>
            <div class="row g-2 mb-1">
                <div class="col-md-6">
                    <div class="d-flex flex-column">
                        <label for="Email">Email (Used to send receipt)</label>
                        <input id="Email" type="email" v-model="pmr.Email"/>
                    </div>
                </div>
            </div>
            <div class="row g-2 mb-1">
                <div class="col-md-10">
                    <div class="d-flex flex-column">
                        <label for="Comments">Comments (Optional, will print on receipt)</label>
                        <input id="Comments" v-model="pmr.Comments"/>
                    </div>
                </div>
            </div>
            <br>

            <div class="form-group row">
                <div class="col-offset-4 col-md-2">
                    <button type="submit" class="btn btn-primary" >Continue to Payment</button>
                </div>
            </div>
        </div>
        </form>

        </main>`
})
app.component('Confirm', {
    props: {
        contextPath: String,
        payMaineRequest: Object,
        messages: Array
        // TODO: receive fieldErrors from GET
    },
    data() {
        return {
            msgs: this.messages,
            pmr: this.payMaineRequest,
            ti: this.payMaineRequest.TrackingInfo
        }
    },
    computed: {
        people() {
            var result = "";
            var lst = this.pmr.TrackingInfo.People;
            for (var i = 0; i < lst.length; i++) {
                result += lst[i] + "\n";
            }
            return result;
        }
    },
    methods: {
        checkout() {
            window.location.replace(this.contextPath + "PaymentInfo/Edit?TrackingNbr=" + this.pmr.TrackingInfo.TrackingNbr);
        }
    },
    template:
        /*html*/
        `<main id="main" class="my-2 pb-3">
        <alert v-for="message in msgs" :status="message.status" :content="message.content"></alert>

        <form v-on:submit.prevent="checkout">

        <h3>Validate Tracking Number Information</h3>
        <span>(If this information is incorrect please contact your LUPC staff contact)</span>
        <br>
        <br>
        <div class="row g-2 mb-2">
            <div class="col-md-3">
                <div class="d-flex flex-column">
                    <label for="TrackingNbr">Tracking Number</label>
                    <input id="TrackingNbr" v-model="pmr.TrackingInfo.TrackingNbr" readonly tabindex="-1"/>
                </div>
            </div>
            <div class="col-md-3">
                <div class="d-flex flex-column">
                    <label for="StaffMember">Staff Member</label>
                    <input id="StaffMember" v-model="pmr.TrackingInfo.StaffMember" readonly tabindex="-1"/>
                </div>
            </div>
            <div class="col-md-3">
                <div class="d-flex flex-column">
                    <label for="People">Applicant(s)</label>
                    <textarea id="People" readonly>{{people}}</textarea>
                </div>
            </div>
            <div class="col-md-3">
                <div class="d-flex flex-column">
                    <label for="Town">Town</label>
                    <input id="Town" v-model="pmr.TrackingInfo.Town" readonly tabindex="-1"/>
                </div>
            </div>
        </div>
        <div class="row g-2 mb-2">
            <div class="col-md-6">
                <div class="d-flex flex-column">
                    <label for="AppTypeNbr">Application Type and Number (if known)</label>
                    <input id="AppTypeNbr" v-model="pmr.TrackingInfo.ApplicationTypeNbr" readonly tabindex="-1"/>
                </div>
            </div>
            <div class="col-md-3">
                <div class="d-flex flex-column">
                    <label v-if="ti.BalanceDueIsCertificatesOfCompletion" for="TotalFee">Total COC Fee</label>
                    <label v-else for="TotalFee">Total Fee</label>
                    <input id="TotalFee" v-model="pmr.TrackingInfo.TotalFee" readonly tabindex="-1"/>
                </div>
            </div>
            <div class="col-md-3">
                <div class="d-flex flex-column">
                    <label for="balanceDue">Balance Due</label>
                    <input id="balanceDue" v-model="ti.BalanceDue" readonly/>
                </div>
            </div>
        </div>
        <br>
        <div class="form-group row">
            <div class="col-offset-4 col-md-2">
                <button v-if='pmr.TrackingInfo.TrackingNbr' type="submit" class="btn btn-primary" >Checkout</button>
            </div>
            <div class="col-offset-1 col-md-5">
                <a :href="contextPath+'TrackingNbr/Select'" class="fs-4 nav-link" >Back</a>
            </div>
        </div>
    </form>

    </main>`
})
app.component('lupc-header', {
    props: {
        contextPath: String
    },
    
    template:
        /*html */
        `<div class="row g-0 align-items-center justify-content-center navbar-inverse">
            <div class="col-md-auto text-center">
                <div class="row align-items-center justify-content-center my-1">
                    <div class="col-auto fs-2 fw-bold">
                        <span class="text-primary"></span><span>&nbsp;LUPC Application Payments</span>
                    </div>

                </div>
            </div>
            <div class="col-md">
                <nav role="navigation"
                    class="nav justify-content-center justify-content-md-start d-print-none ms-md-4">
                    <a :href="contextPath+'ApplicationFee/Pay'" class="fs-4 nav-link" >Home</a>
                    <a :href="contextPath+'Home/About'" class="fs-4 nav-link" >About</a>
                </nav>
            </div>
        </div>
        <br>
    `
})
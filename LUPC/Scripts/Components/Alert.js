app.component('alert', {
    props: {
        status: String,
        content: String,
    },
    template:
        /*html*/
        `<div class="alert" :class="'alert-'+status" role="alert">
          {{content}}
        </div>`
})

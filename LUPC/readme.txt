LUPC

This application was created as an MVC Framework website at version 4.7.2 with Visual Studio 2019 in November 2021.
It used entity framework 6.0.

Using nuget package manager, bootstrap was upgraded to 5.0 and all jquery packages were removed.
Vue js files were added to script manually.  (There is no need for jquery dom manipulation when using vue)
We're not using bootstrap.js and the scripts.render statement for this was removed from _layout.cshtml.

This app uses Bootstrap 5.  
class="d-flex flex-column" is used to align lables above input boxes.

Invisible recaptcha is used on the products form to detect bots.
https://developers.google.com/recaptcha/docs/v3
The site key is recorded in the script tag (see shared\_layout.cshtml) and the Javascript call (scripts\TrackingNbr\select.js).
Not sure why it is needed in both places.
Keys are registered here:
https://www.google.com/recaptcha/admin/site/443457877/settings

If you have trouble with this, you can always register a new key.

This app uses Bootstrap 5.  
class="d-flex flex-column" is used to align lables above input boxes.

This application shares a database with the Goat application.  Goat cannot be modified, so existing table structures must
remain intact.  For this reason, the Goat Check_Record table has been extended with CHECKRECORD_PAYMENT_SUPPLEMENT.

This application has one primary use case: paying the balance due on a tracking number.

Flow of forms:
Published entry point:	ApplicationFee/Pay
Forms:
TrackingNbr/select		User enters tracking number
		   /confirm		Display information about the tracking number to confirm
PaymentInfo/edit		Payment method, contact information
		   /postData	Updates LUPC tables
Payment/RouteToBank		Client does the routing.  See Scripts/Components/PaymentInfo/Edit.js
PaymentResponse/Success
			    Declined
				Cancelled


Application changes
July 2022
Convert to the new database from Chris Halsted's application rewrite.
Server name and port is W-B1VB-19-EXP1,1833.  Unable to connect with this, using IP address in web.config instead.

June 2022
- Send email to the staff member as well as the LUPC office when they are different.
- Don't send email every time the receipt is refereshed, just send once the first time.
- Reuse a check record entry if there is an existing entry without a successful payment.

March 2022
- Use the Pay Maine confirmStatus API to determine if a ticket is in Payment Pending status rather than doing it in LUPC.
- Revise the receipt to use the client payment id instead of the tracking number.  This will only make a difference when it 
is a receipt for a partial payment of the tracking number.


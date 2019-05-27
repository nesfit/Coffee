<template>
    <div>
      <h1>Card Assignment</h1>

      <h2>Lookup by RFID card serial number</h2>
      <b-form @submit="performLookup">
         <b-form-group label="RFID card serial number:">
            <b-form-input v-model="lookupSerialNo" type="text" required />
        </b-form-group>

        <b-button type="submit">Lookup</b-button>
      </b-form>

      <h2>Assign card to user</h2>
      <b-form @submit="assignCardToUser">
        <b-form-group label="RFID card serial number:">
          <b-form-input v-model="toUserSerialNo" type="text" required />
        </b-form-group>

        <UserSelector v-model="toUserId" label="User" v-bind:required="true" />
        <b-button type="submit" v-bind:disabled="isAssignToUserDisabled">Assign</b-button>
      </b-form>

      <h2>Revoke card assignments to user</h2>
      <b-form @submit="revokeCardsOfUser">
        <UserSelector v-model="delAllUserId" label="User" v-bind:required="true" />
        <b-button type="submit" v-bind:disabled="isRevokeDisabled">Assign</b-button>
      </b-form>

      <b-modal id="assignmentDetails" title="Card Assignment Details">
        <p>Serial number: {{ lookupSerialNo }}</p>

        <p v-if="assignmentDetails.loading">Loading..</p>
        <div v-else>
          <p v-if="assignmentDetails.meansId">Card present in database.</p>
          <p v-else>Card not present in database.</p>

          <p v-if="assignmentDetails.userId">
            Currently assigned to user.<br>
            Assignment ID: {{ assignmentDetails.assignmentId }}<br>
            User ID: {{ assignmentDetails.userId }}<br>
            <UserName v-if="assignmentDetails.userId" v-bind:id="assignmentDetails.userId" />
          </p>
          <p v-else>Currently not assigned to a user.</p>
        </div>

        <template slot="modal-footer">
          <b-button variant="danger" v-if="assignmentDetails.assignmentId" @click="cancelAssignment">Cancel assignment</b-button>
        </template>
      </b-modal>

      <b-modal id="revocationProgress" title="Revocation Progress" hide-footer>
        <p v-if="revocationInProgress">Loading and revoking cards of user, {{ revokedCount }}/{{ revokedTotal}} cards revoked so far.</p>
        <p v-else>Done, revoked {{ revokedCount }} cards.</p>
      </b-modal>
  </div>
</template>

<script>

import UserName from '@/components/Display/UserName.vue'
import UserSelector from '@/components/Selection/UserSelector.vue'

export default {
  name: 'CardAssignment',
  components: {UserName,UserSelector},
  data: function() {
    return { 
      lookupSerialNo: "",
      
      assignmentDetails: {},

      toUserSerialNo: "",
      toUserId: "",

      delAllUserId: "",
      revocationInProgress: false,
      revokedCount: 0,
      revokedTotal: 0
    };
  },
  computed: {
    isAssignToUserDisabled: function() {
      return !this.toUserSerialNo || !this.toUserId;
    },
    isRevokeDisabled: function() {
      return !this.delAllUserId;
    }
  },
  methods: {
    performLookup() {
      var c = this;

      c.assignmentDetails = {loading: true};
      c.$bvModal.show("assignmentDetails");

      c.$api
        .get("authenticationMeans", { params: { method: "MFRC522SerialNumber", value: c.lookupSerialNo }})
        .then(response => {
          if (response.data.items.length == 1) {
            c.assignmentDetails.meansId = response.data.items[0].id;
            c.performAssignmentsLookup(c.assignmentDetails.meansId);
          }
          else {
            c.assignmentDetails.loading = false;
          }          
        })
        .catch(() => {
          c.$bvModal.msgBoxOk('Card lookup failed');
          c.assignmentDetails.loading = false;
        });
    },

    performAssignmentsLookup(meansId) {
      var c = this;

      c.$api
        .get("assignmentsToUser", {params: { mustBeValid: true, ofAuthenticationMeans: meansId }})
        .then(response => {
          if (response.data.items.length == 1) {
            c.assignmentDetails.assignmentId = response.data.items[0].id;
            c.assignmentDetails.userId = response.data.items[0].assignedToUserId;
          }

          c.assignmentDetails.loading = false;         
        })
        .catch(() => {
           c.$bvModal.msgBoxOk("Card assignment lookup failed");
           c.assignmentDetails.loading = false;
        });
    },

    cancelAssignment() {
      var c = this;

      c.$api
        .delete("assignmentsToUser/" + c.assignmentDetails.assignmentId)
        .then(() => {
          c.$bvModal.hide("assignmentDetails");  
          c.$bvModal.msgBoxOk("Card was unassigned successfully");
        })
        .catch(() => {
           c.$bvModal.msgBoxOk("Card unassignment failed");
        });
    },

    assignCardToUser(evt) {
      evt.preventDefault();
      var c = this;

      c.$api.get("authenticationMeans/?method=MFRC522SerialNumber&value=" + c.toUserSerialNo)
        .then(resp => {
          if (resp.data.items && resp.data.items.length == 1) {
            c.assignMeansToUser(resp.data.items[0].id, c.toUserId);
          }
          else {
            c.$api.post("authenticationMeans", {
              type: "MFRC522SerialNumber",
              value: c.toUserSerialNo
            }).then(meansResp => {
              c.assignMeansToUser(meansResp.data.id, c.toUserId);
            }); 
          }
        });
    },

    assignMeansToUser(meansId, userId) {
      var c = this;

      c.$api.post("assignmentsToUser", { meansId: meansId, userId: userId, isShared: false })
        .then(() => c.$bvModal.msgBoxOk("Card was assigned successfully"))
        .catch(() => c.$bvModal.msgBoxOk("Card assignment failed"));
    },
    
    revokeCardsOfUser(evt) {
      evt.preventDefault();
      var userId = this.delAllUserId;
      this.revocationInProgress = true;
      this.revokedTotal = 0;
      this.revokedCount = 0;
      this.$bvModal.show("revocationProgress");

      var c = this;

      var failedCount = 0;
      
      c.$api.get("users/" + userId + "/assignedMeans/?method=MFRC522SerialNumber&resultsPerPage=50")
        .then(resp => {
          var assignedCards = resp.data.items;
          
          if (assignedCards) {
            c.revokedTotal = assignedCards.length;

            if (c.revokedTotal == 0)
              c.revocationInProgress = false;
            else {
              for (var i = 0; i < assignedCards.length; i++) {
                var assignedCard = assignedCards[i];

                c.$api.get("assignmentsToUser/?ofAuthenticationMeans="+assignedCard.id+"&assignedToUser="+userId)
                  .then(resp => {
                    if (resp.data.items.length == 1) {
                      var ass = resp.data.items[0];

                      c.$api.delete("assignmentsToUser/" + ass.id)
                        .then(() => {
                          c.revokedCount++;
                          if ((c.revokedCount + failedCount) == c.revokedTotal)
                            c.revocationInProgress = false;
                        })
                        .catch(() => {
                          failedCount++;
                          c.$bvModal.msgBoxOk('Could not revoke card with assignment ID ' + ass.id);
                          if ((c.revokedCount + failedCount) == c.revokedTotal)
                            c.revocationInProgress = false;
                        });
                    }
                  });
              }                
            }
          }
        }).catch(() => {
          c.revocationInProgress = false;
          c.$bvModal.msgBoxOk('Could not generate revocation list');
        });
    }
  }
}
</script>

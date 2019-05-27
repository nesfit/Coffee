<template>
  <div class="mt-3">
    <b-form>
        <div class="container">
            <div class="row">
                <div class="col-md-3 col-sm-12">
                    <b-button @click="reload">Refresh</b-button>
                </div>

                <b-form-group label="Results per page" label-cols-sm="6" label-cols-md="4" class="col-md-9 col-sm-12">
                    <b-form-select v-model="resultsPerPage" :options="resultsPerPageOptions" @change="resultsPerPageChanged" />
                </b-form-group>
            </div>
        </div>
    </b-form>

    <Message v-bind:data="alert" />

    <b-table @row-clicked="tableRowClicked" :sort-by="startingSortBy" :sort-direction="startingSortDir" primary-key="id" striped hover :items="items" :fields="fields" v-if="pageCount > 0" @sort-changed="sortingChanged" no-local-sorting>
        <template v-for="(_, slot) of $scopedSlots" v-slot:[slot]="scope">
            <slot :name="slot" v-bind="scope"/>
        </template>
    </b-table>

     <b-pagination
      v-model="currentPage"
      v-bind:total-rows="resultCount"
      v-bind:per-page="resultsPerPage"
      v-if="resultCount > 0"
      @input="loadPage"
    ></b-pagination>
  </div>
</template>

<script>
export default {
  name: "PagedQuery",
  props: {
      endpoint: String,
      fields: Array,
      startingSortBy: String,
      startingSortDir: String,
      additionalQueryParams: Object
  },
  data: function() {
      return {
          items: [],
          alert: {},
          pageCount: 0,
          currentPage: 1,
          resultCount: 0,
          resultsPerPage: 50,
          lastResultsPerPage: 5,
          isLoading: false,
          resultsPerPageOptions: [ 5, 10, 20, 30, 50, 100 ],
          sortRequests: []
      }
  },
  methods: {
      resultsPerPageChanged: function() {
          if (this.resultsPerPage != this.lastResultsPerPage) {
              this.lastResultsPerPage = this.resultsPerPage;
              this.loadPage(this.currentPage);
          }
      },
      sortingChanged(ctx) {
          if (ctx.sortBy === null)
            this.sortRequests = [];
          else {
            this.sortRequests = [ctx.sortBy + " " + (ctx.sortDesc ? "desc" : "asc")];
            this.loadPage(this.currentPage);
          }        
      },
      tableRowClicked(record, index) {
        this.$emit("item-clicked", record, index);  
      },
      loadPage: function(pageNo) {
        var c = this;

        if (c.isLoading)
            return;

        c.isLoading = true;
        c.alert = {"info": "Loading.."};
        c.currentPage = pageNo;

        var queryParams = { resultsPerPage: c.resultsPerPage, currentPage: c.currentPage };

        if (c.sortRequests.length) {
            queryParams.sortBy = c.sortRequests;
        }

        if (c.additionalQueryParams) {
            var additionalQPKeys = Object.keys(c.additionalQueryParams);
            for (var i = 0; i < additionalQPKeys.length; i++) {
                var key = additionalQPKeys[i];
                var value = c.additionalQueryParams[key];

                if (value == null)
                    continue;

                queryParams[key] = value;
            }
        }

        c.$api.get(c.endpoint, { params: queryParams })
            .then(response => {
                c.items = response.data.items;
                c.pageCount = response.data.totalPages;
                c.currentPage = response.data.currentPage;
                c.resultCount = response.data.totalResults;
                c.alert = c.pageCount == 0 ? {"warning":"No results."} : {};
            })
            .catch(() => c.alert = {"danger": "Error loading items"})
            .then(() => c.isLoading = false);
      },
      reload() {
          this.loadPage(1);
      }
  },
  mounted() {
      this.reload();
  },
  watch: {
      additionalQueryParams() {
          //debugger;
          this.loadPage(this.currentPage);
      }
  }
}
</script>

<style scoped>
</style>
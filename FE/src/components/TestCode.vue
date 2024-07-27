<template>
  <div>
    <h1>LaTeX to PDF Converter</h1>
    <textarea
      class="editor"
      v-model="latexCode"
      placeholder="Enter your LaTeX code here"
    ></textarea>
    <button @click="convertLatex">Convert to PDF</button>
    <div class="error-output" v-if="errorMessage">
      <pre>{{ errorMessage }}</pre>
      <!-- <pre>{{ errorDetails }}</pre> -->
    </div>
    <div v-if="pdfData">
      <a :href="pdfData" target="_blank">Download PDF</a>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      latexCode: "",
      errorMessage: "",
      //errorDetails: "",
      pdfData: null,
    };
  },
  methods: {
    async convertLatex() {
      this.errorMessage = null;
      //this.errorDetails = null;
      this.pdfData = null;

      try {
        const response = await axios.post(
          "http://localhost:5025/api/Latex/convert",
          { latexCode: this.latexCode },
          { responseType: "blob" }
        );
        if (response.status !== 200) {
          const text = await response.data.text();
          const jsonResponse = JSON.parse(text);
          this.errorMessage = `Message: ${jsonResponse.message}, Details: ${jsonResponse.details}`;
          return;
        }
        this.pdfData = URL.createObjectURL(response.data);
      } catch (error) {
        if (error.response && error.response.data instanceof Blob) {
          const text = await error.response.data.text();
          const jsonResponse = JSON.parse(text);
          this.errorMessage = `Message: ${jsonResponse.message}, Details: ${jsonResponse.details}`;
        } else {
          this.errorMessage = `Error: ${error.message}`;
        }
      }
    },
  },
  // computed: {
  //   pdfUrl() {
  //     if (this.pdfData) {
  //       const blob = new Blob([this.pdfData], { type: "application/pdf" });
  //       return URL.createObjectURL(blob);
  //     }
  //     return null;
  //   },
  // },
};
</script>

<style>
.editor {
  width: 600px;
  height: 500px;
}

.error-output {
  width: 500px;
  height: 500px;
  background-color: blanchedalmond;
}
</style>

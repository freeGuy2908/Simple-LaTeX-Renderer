<template>
  <div class="container">
    <!-- top tool bar -->
    <div class="tool-bar">
      <input type="file" @change="handleFileUpload" multiple />
      <button id="compile-btn" @click="convertLatexToPdf">Compile</button>
      <button class="output-btn" @click="toggleView">
        <i class="fa-regular fa-file"></i>
      </button>
    </div>
    <!-- container 1 chua file list va code+pdf-->
    <div class="c1">
      <div class="file-list">
        <ul>
          <li v-for="(file, index) in uploadedFiles" :key="index">
            <button class="file-btn" @click="loadFileContent(file)">
              {{ file.name }}
            </button>
          </li>
        </ul>
      </div>
      <!-- container 2 chua code+pdf -->
      <div class="c2">
        <!-- code editor -->
        <div class="code-editor">
          <Codemirror
            ref="editor"
            v-model:value="latexCode"
            @submit.prevent="convertLatexToPdf"
            :options="cmOptions"
            border
            placeholder="Enter code here"
          />
        </div>
        <!-- pdf viewer -->
        <div class="pdf-viewer" v-if="pdfUrl">
          <iframe id="pdf-here" :src="pdfUrl"></iframe>
        </div>
      </div>
    </div>
  </div>
  <div class="error-output" v-if="errorMessage">
    <pre>{{ errorMessage }}</pre>
  </div>
</template>

<script>
import Codemirror from "codemirror-editor-vue3";
import "codemirror/addon/display/placeholder.js";
import "codemirror/mode/stex/stex.js";
import "codemirror/addon/display/placeholder.js";
import "codemirror/theme/dracula.css";

import axios from "axios";

//import { ref } from "vue";
export default {
  components: { Codemirror },
  setup() {
    return {
      cmOptions: {
        mode: "text/x-latex",
        theme: "default",
      },
    };
  },
  data() {
    return {
      latexCode: "",
      pdfUrl: "",
      uploadedFiles: [],
      showOutput: false,
      errorMessage: "",
    };
  },
  methods: {
    handleFileUpload(event) {
      const files = Array.from(event.target.files);
      this.uploadedFiles.push(...files);
    },
    loadFileContent(file) {
      if (file.name.endsWith(".tex")) {
        const reader = new FileReader();
        reader.onload = (e) => {
          this.latexCode = e.target.result;
          // this.errorMessage = "";
        };
        reader.readAsText(file);
      }
    },
    async convertLatexToPdf() {
      try {
        const response = await axios.post(
          "http://localhost:5215/api/Latex/convert",
          { latexCode: this.latexCode },
          { responseType: "blob" }
        );
        if (response.status !== 200) {
          const text = await response.data.text(); // chuyen Blob thanh text
          const jsonResponse = JSON.parse(text); // phan tich cu phap text thanh doi tuong json
          // sau do co the truy cap duoc vao field message va details
          this.errorMessage = `Message: ${jsonResponse.message}, Details: ${jsonResponse.details}`;
          return;
        }
        this.pdfUrl = URL.createObjectURL(response.data);
      } catch (error) {
        if (error.response && error.response.data instanceof Blob) {
          const text = await error.response.data.text();
          const jsonResponse = JSON.parse(text);
          this.errorMessage = `${jsonResponse.message},
                              ${jsonResponse.details}`;
        } else {
          this.errorMessage = `Error: ${error.message}`;
        }
      }
    },
    toggleView() {
      this.showOutput = !this.showOutput;
    },
    // downloadPdf() {
    //   const link = document.createElement("a");
    //   link.href = this.pdfUrl;
    //   link.download = "document.pdf";
    //   document.body.appendChild(link);
    //   link.click();
    //   document.body.removeChild(link);
    // },
  },
};
</script>

<style>
@import "~@fortawesome/fontawesome-free/css/all.css";

.container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.tool-bar {
  width: 100%;
  background-color: #495365;
  padding: 10px 0 10px 0;
  margin-bottom: 10px;
  right: 0;
}

.c1 {
  display: flex;
  justify-content: space-between;
  width: 100%;
}

.file-list,
.c2 {
  background-color: #495365;
  height: 90vh;
  padding: 8px;
  box-sizing: border-box;
}

.c2 {
  /* position: fixed; */
  display: flex;
  justify-content: space-between; /* Distributes space between items */
  align-items: center; /* Centers items vertically if they have different heights */
  width: 86%;
  bottom: 0;
  right: 0;
}

.code-editor {
  height: 86vh;
  width: 50%;
  margin-right: 8px;
}

#compile-btn,
.file-btn,
.output-btn {
  cursor: pointer;
  color: white;
  background-color: #04aa6d;
  font-size: 16px;
  padding: 4px 4px;
  border-radius: 5px;
}

.pdf-viewer {
  height: 86vh;
  width: 50%;
}

.code-editor .CodeMirror {
  font-size: 16px;
}

#pdf-here {
  width: 100%;
  height: 100%;
  background-color: white;
}

.error-output {
  background-color: #495365;
  color: white;
}

.file-list {
  /* position: fixed; */
  width: 13%;
  bottom: 0;
  margin-right: 10px;
}
</style>

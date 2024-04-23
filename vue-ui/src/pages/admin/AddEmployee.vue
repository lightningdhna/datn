<script setup lang="ts">
import { VForm } from 'vuetify/components/VForm'
import { formatCamelCase } from '@/@core/utils/formatters'
import { emailValidator, phoneNumberValidator, requiredValidator } from '@/@core/utils/validator'
import { addEmployee } from '@/services/api/employeeApi'
import { getPositionList } from '@/services/employeeDataFetching'

const positionList = ref([] as string[])

getPositionList().then(positions => {
  positionList.value = positions
})

const numberedSteps = [
  {
    title: 'Cơ bản',
    subtitle: 'Thông tin cơ bản ',
  },
  {
    title: 'Liên lạc',
    subtitle: 'Thông tin liên lạc',
  },
  {
    title: 'Vị trí',
    subtitle: 'Thông tin công việc',
  },

  // {
  //   title: 'Thanh toán',
  //   subtitle: 'Thông tin thanh toán',
  // },
]

const showDialog = ref(false)
const submitSuccess = ref(true)

const refEmployeeForm = ref<InstanceType<typeof VForm>>() as Ref<InstanceType<typeof VForm>>
const refContactForm = ref<InstanceType<typeof VForm>>() as Ref<InstanceType<typeof VForm>>
const refJobForm = ref<InstanceType<typeof VForm>>() as Ref<InstanceType<typeof VForm>>
const refPaymentForm = ref<InstanceType<typeof VForm>>() as Ref<InstanceType<typeof VForm>>
const formRefList: Ref<InstanceType<typeof VForm>>[] = [refEmployeeForm, refContactForm, refJobForm, refPaymentForm]

const employeeForm = ref({
  firstName: '',
  surname: '',
  birthday: '',
  gender: 'Nam',
})

const contactForm = ref({
  email: '',
  phoneNumber: '',
  address: '',
})

const jobForm = ref({
  position: [] as string[],
  department: '',
})

// const paymentForm = ref({
//   cardNumber: '',
//   cardExpiration: '',
//   cardCvv: '',
// })

const resultForm: { employeeId: string; username: string; password: string } = {
  employeeId: '',
  username: 'abc',
  password: 'de',

}

interface FormRef {
  value: {
    validate: () => Promise<{ valid: boolean }>
  }
}

const validateCurrentForm = async (currentStep: number): Promise<boolean> => {
  if (currentStep >= formRefList.length)
    return false

  const formRef: FormRef = formRefList[currentStep]

  if (formRef.value) {
    console.log('it actually find a form re')

    const valid = await formRef?.value?.validate()

    return valid.valid
  }

  return false
}

const onFinish = async () => {
  // console.log('submitting value, sending request to server...')
  // console.log({
  //   ...employeeForm.value,
  //   ...contactForm.value,
  //   ...jobForm.value,

  //   // ...paymentForm.value,
  // })
  addEmployee({
    ...employeeForm.value,
    ...contactForm.value,
    ...jobForm.value,
  })
  showDialog.value = true
}
</script>

<template>
  <Stepper
    :numbered-steps="numberedSteps"
    :validator="validateCurrentForm"
    :on-finish="onFinish"
  >
    <VWindowItem>
      <VBtn @click="showDialog = true" />
      <VForm ref="refEmployeeForm">
        <VRow>
          <VCol cols="12">
            <h6 class="text-sm font-weight-medium">
              Thông tin cơ bản của nhân viên
            </h6>
            <p class="text-xs mb-0">
              Điền các thông tin cơ bản của nhân viên
            </p>
          </VCol>

          <VCol
            cols="12"
            md="4"
          >
            <VTextField
              v-model="employeeForm.surname"
              placeholder="Họ Và"
              :rules="[requiredValidator]"
              label="Họ "
            />
          </VCol>

          <VCol
            cols="12"
            md="4"
          >
            <VTextField
              v-model="employeeForm.firstName"

              placeholder="Tên"
              :rules="[requiredValidator]"
              label="Tên"
            />
          </VCol>
          <VCol
            cols="12"
            md="4"
          >
            <CopiableText
              tile
              :value="`${employeeForm.surname} ${employeeForm.firstName}`"
              :rules="[requiredValidator]"
              label="Họ và tên"
            />
          </VCol>
          <VCol
            cols="12"
            md="4"
          >
            <VTextField
              v-model="employeeForm.birthday"
              :rules="[requiredValidator]"
              label="Ngày sinh"
              type="date"
            />
          </VCol>
          <VCol
            cols="12"
            md="4"
          >
            <VAutocomplete
              v-model="employeeForm.gender"
              label="Giới tính"
              :items="['Nam', 'Nữ']"
              :rules="[requiredValidator]"
            />
          </VCol>
        </VRow>
      </VForm>
    </VWindowItem>
    <VWindowItem>
      <VForm ref="refContactForm">
        <VRow>
          <VCol cols="12">
            <h6 class="text-sm font-weight-medium">
              Thông tin liên lạc
            </h6>
            <p class="text-xs mb-0">
              Thêm thông tin liên lạc tới nhân viên
            </p>
          </VCol>

          <VCol
            cols="12"
            md="7"
          >
            <VTextField

              v-model="contactForm.email"
              prepend-inner-icon="ri-mail-fill"
              label="Email"
              type="email"
              placeholder="johndoe@example.com"
              :rules="[emailValidator]"
              clearable
            />
          </VCol>

          <VCol
            cols="12"
            md="7"
          >
            <VTextField
              v-model="contactForm.phoneNumber"
              prepend-inner-icon="ri-smartphone-fill"
              label="Số điện thoại"
              placeholder="0987654321"
              :rules="[phoneNumberValidator]"
              clearable
            />
          </VCol>
          <VCol
            cols="12"
            md="7"
          >
            <VTextField
              v-model="contactForm.phoneNumber"
              prepend-inner-icon="ri-map-pin-fill"
              label="Địa chỉ"
              placeholder=""
              clearable
            />
          </VCol>
        </vrow>
      </VForm>
    </VWindowItem>
    <VWindowItem>
      <VForm ref="refJobForm">
        <VRow>
          <VCol cols="12">
            <h6 class="text-sm font-weight-medium">
              Công việc
            </h6>
            <p class="text-xs mb-0">
              Điền các thông tin công việc cho nhân viên
            </p>
          </VCol>

          <VCol
            cols="12"
            md="6"
          >
            <VAutocomplete
              v-model="jobForm.position"
              multiple
              label="Vị trí"
              :items="positionList"
              placeholder="Chọn các vị trí"
              :rules="[v => !!v.length || 'At least one option must be selected.']"
              prepend-icon-inner="ri-briefcase-fill"
              clearable
            />
          </VCol>

          <VCol
            cols="12"
            md="6"
          >
            <VTextField
              v-model="jobForm.department"
              placeholder=""
              :rules="[]"
              label="Đơn vị"
              prepend-icon-inner="ri-home-office-fill"
            />
          </VCol>
        </VRow>
      </VForm>
    </VWindowItem>
  </Stepper>
  <VDialog
    v-model="showDialog"
    class="my-dialog"
  >
    <VCard
      fill-height
      class="dialog-card flex-grow-1"
    >
      <VCardTitle>
        <h5
          v-if="submitSuccess"
          class="text-h3 font-weight-semibold mb-1 text-center"
          style="color: #0e739c;"
        >
          Đã thêm nhân viên
        </h5>
        <div v-else>
          <h5
            class="text-h3 font-weight-semibold mb-1 text-center"
            style="color: red; margin-block-end: 20px !important;"
          >
            Hệ thống bị lỗi!
          </h5>
          <h1
            class="text-subtitle-1 font-weight-semibold mb-1 text-center"
            style="font-size: 1em !important;"
          >
            Có vấn đề với server khi cố gắng thêm thông tin nhân viên!<br>
            Vui lòng thử lại sau
          </h1>
        </div>
      </VCardTitle>
      <VCardText v-if="submitSuccess">
        <VRow
          v-for="(value, prop) in resultForm"
          :key="prop"
          class="mt-6"
        >
          <VCol
            cols="12"
            md="4"
            class="d-flex align-center "
          >
            <div class="ml-3">
              {{ formatCamelCase(prop) }}
            </div>
          </VCol>
          <VCol
            cols="12"
            md="8"
            class="d-flex align-self-start"
          >
            <CopiableText
              :value="value"
              style=" inline-size: 90%;"
            />
          </VCol>
        </VRow>
      </VCardText>

      <div
        style="
                display:flex; margin-block-end:40px;
                margin-inline-start:50px;"
      >
        <VBtn
          :rounded="0"
          color="secondary"
          variant="text"
          class="back-button"
          :href="$route.path"
        >
          <VIcon
            icon="ri-arrow-left-line"
            start
          />
          Quay lại
        </VBtn>

        <a
          color="error"
          class="buy-now-button"
          rel="noopener noreferrer"
          :href="submitSuccess ? $route.path : 'javascript:void(0)'"
          @click="showDialog = false"
        >
          Close

        </a>
      </div>
    </VCard>
  </VDialog>
</template>

<style scoped>
.my-dialog  {
  position: fixed;
  margin: auto;
  block-size: 800px;
  inline-size: 600px;
}

.dialog-card {
  block-size:800px;
  inline-size: 600px;
}
</style>

<style lang="scss" scoped>
.back-button{
  display:flex;
  align-items: center;
  cursor: pointer;

  &:hover :deep(.v-btn__content) {
    color: #2b75ad;
  }

}

.buy-now-button {
  position: fixed;
  display: inline-flex;
  box-sizing: border-box;
  align-items: center;
  justify-content: center;
  border: 0;
  border-radius: 6px;
  margin: 0;
  animation: anime 12s linear infinite;
  appearance: none;
  background: linear-gradient(-45deg, #338aff, #3cf0c5, #338aff, #3cf0c5);
  background-size: 600%;
  color: rgba(255, 255, 255, 90%);
  cursor: pointer;
  font-size: 0.9375rem;
  font-weight: 500;
  inset-inline-end:0;
  letter-spacing: 0.43px;
  line-height: 1.2;
  min-inline-size: 100px;
  outline: 0;
  padding-block: 0.625rem;
  padding-inline: 1.25rem;
  text-decoration: none;
  text-transform: none;
  vertical-align: middle;

  &:hover {
    color: white;
    text-decoration: none;
  }

}

@keyframes anime {
  0% {
    background-position: 0% 50%;
  }

  50% {
    background-position: 100% 50%;
  }

  100% {
    background-position: 0% 50%;
  }
}
</style>

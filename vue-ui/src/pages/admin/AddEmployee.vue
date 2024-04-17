<script setup lang="ts">
import { VForm } from 'vuetify/components/VForm'
import { emailValidator, requiredValidator } from '@/@core/utils/validator'
import Stepper from '@core/components/Stepper.vue'

const numberedSteps = [
  {
    title: 'Cơ bản',
    subtitle: 'Thông tin cơ bản ',
  },
  {
    title: 'Liên lạc',
    subtitle: 'Thêm thông tin liên lạc',
  },
  {
    title: 'Vị trí',
    subtitle: 'Thêm thông tin công việc',
  },
  {
    title: 'Thanh toán',
    subtitle: 'Thêm thông tin thanh toán',
  },
]

const refEmployeeForm = ref<InstanceType<typeof VForm>>() as Ref<InstanceType<typeof VForm>>
const refContactForm = ref<InstanceType<typeof VForm>>() as Ref<InstanceType<typeof VForm>>
const refJobForm = ref<InstanceType<typeof VForm>>() as Ref<InstanceType<typeof VForm>>
const refPaymentForm = ref<InstanceType<typeof VForm>>() as Ref<InstanceType<typeof VForm>>
const formRefList: Ref<InstanceType<typeof VForm>>[] = [refEmployeeForm, refContactForm, refJobForm, refPaymentForm]

const employeeForm = ref({
  firstName: '',
  surname: '',
  birthday: '',
  gender: '',
})

const contactForm = ref({
  email: '',
  phone: '',
  address: '',
})

const jobForm = ref({
  position: '',
  department: '',
})

const paymentForm = ref({
  cardNumber: '',
  cardExpiration: '',
  cardCvv: '',
})

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
</script>

<template>
  <Stepper
    :numbered-steps="numberedSteps"
    :validator="validateCurrentForm"
  >
    <VWindowItem>
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
              :value="`${employeeForm.surname} ${employeeForm.firstName}`"
              :rules="[requiredValidator]"
              label="Họ và tên"
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
              Thông tin cơ bản của nhân viên
            </h6>
            <p class="text-xs mb-0">
              Điền các thông tin cơ bản của nhân viên
            </p>
          </VCol>

          <VCol
            cols="12"
            md="6"
          >
            <VTextField
              clearable
              placeholder="CarterLeonardo"
              :rules="[requiredValidator]"
              label="Username"
            />
          </VCol>

          <VCol
            cols="12"
            md="6"
          >
            <VTextField
              placeholder="carterleonardo@gmail.com"
              :rules="[requiredValidator, emailValidator]"
              label="Email"
            />
          </VCol>
        </VRow>
      </VForm>
    </VWindowItem>
    <VWindowItem>
      <VForm ref="refJobForm">
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
            md="6"
          >
            <VTextField
              placeholder="CarterLeonardo"
              :rules="[requiredValidator]"
              label="Username"
            />
          </VCol>

          <VCol
            cols="12"
            md="6"
          >
            <VTextField
              placeholder="carterleonardo@gmail.com"
              :rules="[requiredValidator, emailValidator]"
              label="Email"
            />
          </VCol>
        </VRow>
      </VForm>
    </VWindowItem>
    <VWindowItem>
      <VForm ref="refPaymentForm">
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
            md="6"
          >
            <VTextField
              placeholder="CarterLeonardo"
              :rules="[requiredValidator]"
              label="Username"
            />
          </VCol>

          <VCol
            cols="12"
            md="6"
          >
            <VTextField
              placeholder="carterleonardo@gmail.com"
              :rules="[requiredValidator, emailValidator]"
              label="Email"
            />
          </VCol>
        </VRow>
      </VForm>
    </VWindowItem>
  </Stepper>
</template>

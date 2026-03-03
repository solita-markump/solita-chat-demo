import { ref } from 'vue';
import { mockMessages } from '@/mock-data';
import ChatBubble from '@/components/ChatBubble.vue';
import ChatComposer from '@/components/ChatComposer.vue';
const messages = ref([...mockMessages]);
let nextId = messages.value.length + 1;
function handleSend(text) {
    messages.value.push({ id: nextId++, author: 'You', text, isMine: true });
}
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
__VLS_asFunctionalElement(__VLS_intrinsicElements.div, __VLS_intrinsicElements.div)({
    ...{ class: "flex h-full flex-col bg-white" },
});
__VLS_asFunctionalElement(__VLS_intrinsicElements.header, __VLS_intrinsicElements.header)({
    ...{ class: "flex items-center gap-2 border-b border-gray-200 bg-blue-600 px-4 py-3" },
});
__VLS_asFunctionalElement(__VLS_intrinsicElements.h1, __VLS_intrinsicElements.h1)({
    ...{ class: "text-base font-semibold text-white" },
});
__VLS_asFunctionalElement(__VLS_intrinsicElements.main, __VLS_intrinsicElements.main)({
    ...{ class: "flex flex-1 flex-col gap-2 overflow-y-auto p-3" },
});
for (const [msg] of __VLS_getVForSourceType((__VLS_ctx.messages))) {
    /** @type {[typeof ChatBubble, ]} */ ;
    // @ts-ignore
    const __VLS_0 = __VLS_asFunctionalComponent(ChatBubble, new ChatBubble({
        key: (msg.id),
        message: (msg),
    }));
    const __VLS_1 = __VLS_0({
        key: (msg.id),
        message: (msg),
    }, ...__VLS_functionalComponentArgsRest(__VLS_0));
}
/** @type {[typeof ChatComposer, ]} */ ;
// @ts-ignore
const __VLS_3 = __VLS_asFunctionalComponent(ChatComposer, new ChatComposer({
    ...{ 'onSend': {} },
}));
const __VLS_4 = __VLS_3({
    ...{ 'onSend': {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_3));
let __VLS_6;
let __VLS_7;
let __VLS_8;
const __VLS_9 = {
    onSend: (__VLS_ctx.handleSend)
};
var __VLS_5;
/** @type {__VLS_StyleScopedClasses['flex']} */ ;
/** @type {__VLS_StyleScopedClasses['h-full']} */ ;
/** @type {__VLS_StyleScopedClasses['flex-col']} */ ;
/** @type {__VLS_StyleScopedClasses['bg-white']} */ ;
/** @type {__VLS_StyleScopedClasses['flex']} */ ;
/** @type {__VLS_StyleScopedClasses['items-center']} */ ;
/** @type {__VLS_StyleScopedClasses['gap-2']} */ ;
/** @type {__VLS_StyleScopedClasses['border-b']} */ ;
/** @type {__VLS_StyleScopedClasses['border-gray-200']} */ ;
/** @type {__VLS_StyleScopedClasses['bg-blue-600']} */ ;
/** @type {__VLS_StyleScopedClasses['px-4']} */ ;
/** @type {__VLS_StyleScopedClasses['py-3']} */ ;
/** @type {__VLS_StyleScopedClasses['text-base']} */ ;
/** @type {__VLS_StyleScopedClasses['font-semibold']} */ ;
/** @type {__VLS_StyleScopedClasses['text-white']} */ ;
/** @type {__VLS_StyleScopedClasses['flex']} */ ;
/** @type {__VLS_StyleScopedClasses['flex-1']} */ ;
/** @type {__VLS_StyleScopedClasses['flex-col']} */ ;
/** @type {__VLS_StyleScopedClasses['gap-2']} */ ;
/** @type {__VLS_StyleScopedClasses['overflow-y-auto']} */ ;
/** @type {__VLS_StyleScopedClasses['p-3']} */ ;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            ChatBubble: ChatBubble,
            ChatComposer: ChatComposer,
            messages: messages,
            handleSend: handleSend,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
//# sourceMappingURL=App.vue.js.map
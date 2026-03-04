import { ref, onMounted } from 'vue';
import ChatBubble from '@/components/ChatBubble.vue';
import ChatComposer from '@/components/ChatComposer.vue';
import { getMessages, sendMessage, ROOM_ID, AUTHOR_NAME } from '@/api/chat-api';
const messages = ref([]);
const loadError = ref(null);
const sendError = ref(null);
const isLoading = ref(false);
onMounted(async () => {
    isLoading.value = true;
    loadError.value = null;
    try {
        const data = await getMessages(ROOM_ID);
        messages.value = data.items.map((item) => ({
            id: item.id,
            author: item.authorName,
            text: item.text,
            isMine: item.authorName === AUTHOR_NAME,
        }));
    }
    catch {
        loadError.value = 'Could not load messages. Is the backend running?';
    }
    finally {
        isLoading.value = false;
    }
});
async function handleSend(text) {
    sendError.value = null;
    try {
        const item = await sendMessage(ROOM_ID, AUTHOR_NAME, text);
        messages.value.push({
            id: item.id,
            author: item.authorName,
            text: item.text,
            isMine: item.authorName === AUTHOR_NAME,
        });
    }
    catch {
        sendError.value = 'Could not send message. Please try again.';
    }
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
if (__VLS_ctx.isLoading) {
    __VLS_asFunctionalElement(__VLS_intrinsicElements.p, __VLS_intrinsicElements.p)({
        ...{ class: "text-center text-xs text-gray-400" },
    });
}
else if (__VLS_ctx.loadError) {
    __VLS_asFunctionalElement(__VLS_intrinsicElements.p, __VLS_intrinsicElements.p)({
        ...{ class: "text-center text-xs text-red-500" },
    });
    (__VLS_ctx.loadError);
}
else {
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
}
if (__VLS_ctx.sendError) {
    __VLS_asFunctionalElement(__VLS_intrinsicElements.p, __VLS_intrinsicElements.p)({
        ...{ class: "text-center text-xs text-red-500" },
    });
    (__VLS_ctx.sendError);
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
/** @type {__VLS_StyleScopedClasses['text-center']} */ ;
/** @type {__VLS_StyleScopedClasses['text-xs']} */ ;
/** @type {__VLS_StyleScopedClasses['text-gray-400']} */ ;
/** @type {__VLS_StyleScopedClasses['text-center']} */ ;
/** @type {__VLS_StyleScopedClasses['text-xs']} */ ;
/** @type {__VLS_StyleScopedClasses['text-red-500']} */ ;
/** @type {__VLS_StyleScopedClasses['text-center']} */ ;
/** @type {__VLS_StyleScopedClasses['text-xs']} */ ;
/** @type {__VLS_StyleScopedClasses['text-red-500']} */ ;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            ChatBubble: ChatBubble,
            ChatComposer: ChatComposer,
            messages: messages,
            loadError: loadError,
            sendError: sendError,
            isLoading: isLoading,
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